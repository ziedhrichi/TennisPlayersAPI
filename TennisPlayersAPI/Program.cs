using Microsoft.OpenApi.Models;
using TennisPlayersAPI.Data;
using TennisPlayersAPI.Exceptions;
using TennisPlayersAPI.Repositories;
using TennisPlayersAPI.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b =>
    {
        b.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Infos générales sur l’API
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

    // Récupère les commentaires XML générés par /// <summary>
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<IFileSystem, RealFileSystem>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayersService, PlayersService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tennis Player API v1");
    c.RoutePrefix = string.Empty; // Swagger UI accessible à la racine (http://localhost:5000)
});


app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

// Activé le middleware d'exception
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }