using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TennisPlayersAPI.Data;
using TennisPlayersAPI.Exceptions;
using TennisPlayersAPI.Logs;
using TennisPlayersAPI.Repositories;
using TennisPlayersAPI.Services;

var builder = WebApplication.CreateBuilder(args);

#region Configuration (appsettings + variables d’environnement)

// Charge la config depuis appsettings.json + appsettings.{env}.json + variables d’environnement
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

#endregion

#region Authentification & JWT

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Validation du token JWT
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        RoleClaimType = ClaimTypes.Role,               // lecture des rôles
        NameClaimType = JwtRegisteredClaimNames.Sub    // lecture du "sub"
    };

    // Gestion des erreurs d’auth/autorisation
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            // Token manquant ou invalide → 401 JSON custom
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsJsonAsync(new
            {
                errorType = "Unauthorized",
                message = "Authentification requise.",
                playerId = (int?)null
            });
        },
        OnForbidden = context =>
        {
            // Token valide mais pas les droits → 403 JSON custom
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsJsonAsync(new
            {
                errorType = "Forbidden",
                message = "Accès refusé : vous n’avez pas les droits suffisants pour effectuer cette action.",
                playerId = (int?)null
            });
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Read", policy =>
        policy.RequireRole("Visitor", "Editor", "Admin"));

    options.AddPolicy("Write", policy =>
        policy.RequireRole("Editor", "Admin"));

    options.AddPolicy("Delete", policy =>
        policy.RequireRole("Admin"));
});


#endregion

#region Logging avec Serilog

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/tennisplayer-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

#region Services applicatifs

// Contrôleurs REST
builder.Services.AddControllers();

// Politique CORS permissive (ouvert à tout le monde)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b =>
    {
        b.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

// Swagger + documentation API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Infos générales de l’API
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tennis Player API",
        Version = "v1",
        Description = "API de gestion des joueurs de tennis avec documentation Swagger",
        Contact = new OpenApiContact
        {
            Name = "HRICHI ZIED",
            Email = "dsi.zied.hrichi@gmail.com"
        }
    });

    // Ajout des commentaires XML générés par /// <summary>
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Configuration pour sécuriser Swagger avec JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez 'Bearer' [espace] puis votre token.\n\nExemple: \"Bearer eyJhbGciOi...\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Injection des dépendances applicatives
builder.Services.AddScoped<IFileSystem, RealFileSystem>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayersService, PlayersService>();

#endregion


var app = builder.Build();

#region Pipeline HTTP (middlewares)

// Gestion centralisée des exceptions (JSON format)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Logging personnalisé des requêtes HTTP
app.UseMiddleware<RequestLoggingMiddleware>();

// Redirection HTTPS
app.UseHttpsRedirection();

// Autoriser toutes les origines/méthodes/headers (CORS)
app.UseCors("AllowAll");

// Swagger UI (accessible à la racine : http://localhost:5000)
app.UseSwagger();
app.UseSwaggerUI();

// Routing ASP.NET Core
app.UseRouting();

// Authentification & autorisation
app.UseAuthentication();
app.UseAuthorization();

// Mappe les contrôleurs REST
app.MapControllers();

app.Run();

#endregion

// Classe partielle requise pour les tests d’intégration
public partial class Program { }
