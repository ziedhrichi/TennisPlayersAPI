using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using TennisPlayersAPI.Models;

namespace TennisPlayersAPI.IntegrationTests
{
    public class PlayerApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public PlayerApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Crée le client
            _client = factory.CreateClient();

            // Récupère la configuration de l'application
            _config = factory.Services.GetRequiredService<IConfiguration>();
        }

        private string GenerateJwtToken()
        {
            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "admin"),
            new Claim(ClaimTypes.Role, "admin")
        };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Fact]
        public async Task GetPlayers_ReturnsOk_WithList()
        {
            var token = GenerateJwtToken();
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
 
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("api/TennisPlayers");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var players = await response.Content.ReadFromJsonAsync<List<Player>>();
            Assert.NotNull(players);
        }

        [Fact]
        public async Task AddPlayer_ThenGetPlayerById_ReturnsCreatedAndOk()
        {
            var token = GenerateJwtToken();
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var newPlayer = new Player
            {
                Id = 1,
                FirstName = "Roger",
                LastName = "Federer",
                Sex = "Homme",
                Country = new Country { Code = "SUI" },
                Data = new PlayerData
                {
                    Rank = 3,
                    Weight = 185,
                    Height = 85,
                    Last = new List<int> { 1, 0, 1 }
                }
            };
            // POST
            var createResponse = await _client.PostAsJsonAsync("api/TennisPlayers", newPlayer);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var createdPlayer = await createResponse.Content.ReadFromJsonAsync<Player>();
            Assert.NotNull(createdPlayer);

            // GET by id
            var getResponse = await _client.GetAsync($"api/TennisPlayers/{createdPlayer.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var player = await getResponse.Content.ReadFromJsonAsync<Player>();
            Assert.Equal("Federer", player!.LastName);
        }

        [Fact]
        public async Task UpdatePlayer_ReturnsOk()
        {
            var token = GenerateJwtToken();
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var newPlayer = new Player
            {
                Id = 1,
                FirstName = "Rafael",
                LastName = "Nadal",
                Sex = "Homme",
                Country = new Country { Code = "ESP" },
                Data = new PlayerData
                {
                    Rank = 2,
                    Weight = 185,
                    Height = 85,
                    Last = new List<int> { 1, 0, 1 }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("api/TennisPlayers", newPlayer);
            var createdPlayer = await createResponse.Content.ReadFromJsonAsync<Player>();

            // PUT
            var updated = new Player
            {
                Id = 1,
                FirstName = "Rafael",
                LastName = "Nadal",
                Sex = "Homme",
                Country = new Country { Code = "ESP" },
                Data = new PlayerData
                {
                    Rank = 1,
                    Weight = 185,
                    Height = 85,
                    Last = new List<int> { 1, 0, 1 }
                }
            };
            var updateResponse = await _client.PutAsJsonAsync($"api/TennisPlayers/{createdPlayer!.Id}", updated);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var player = await updateResponse.Content.ReadFromJsonAsync<Player>();
            Assert.Equal("Rafael", player!.FirstName);
            Assert.Equal(1, player.Data.Rank);
        }

        [Fact]
        public async Task DeletePlayer_ReturnsNoContent()
        {
            var token = GenerateJwtToken();
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var newPlayer = new Player
            {
                Id = 1,
                FirstName = "***",
                LastName = "Djokovic",
                Sex = "Homme",
                Country = new Country { Code = "SRB" },
                Data = new PlayerData
                {
                    Rank = 1,
                    Weight = 188,
                    Height = 80,
                    Last = new List<int> { 1, 0, 1 }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("api/TennisPlayers", newPlayer);
            var createdPlayer = await createResponse.Content.ReadFromJsonAsync<Player>();

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"api/TennisPlayers/{createdPlayer!.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // GET should return 404
            var getResponse = await _client.GetAsync($"api/TennisPlayers/{createdPlayer.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task GetStatistics_ReturnsOk()
        {
            var token = GenerateJwtToken();
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("api/TennisPlayers/statistics");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var stats = await response.Content.ReadFromJsonAsync<StatisticsResult>();
            Assert.NotNull(stats);
        }


    }
}