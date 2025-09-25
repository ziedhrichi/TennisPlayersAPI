using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using TennisPlayersAPI.Models;

namespace TennisPlayersAPI.IntegrationTests
{
    public class PlayerApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PlayerApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPlayers_ReturnsOk_WithList()
        {
            var response = await _client.GetAsync("/TennisPlayers");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var players = await response.Content.ReadFromJsonAsync<List<Player>>();
            Assert.NotNull(players);
        }

        [Fact]
        public async Task AddPlayer_ThenGetPlayerById_ReturnsCreatedAndOk()
        {
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
            var createResponse = await _client.PostAsJsonAsync("/TennisPlayers", newPlayer);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var createdPlayer = await createResponse.Content.ReadFromJsonAsync<Player>();
            Assert.NotNull(createdPlayer);

            // GET by id
            var getResponse = await _client.GetAsync($"/TennisPlayers/{createdPlayer.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var player = await getResponse.Content.ReadFromJsonAsync<Player>();
            Assert.Equal("Federer", player!.LastName);
        }

        [Fact]
        public async Task UpdatePlayer_ReturnsOk()
        {
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
            var createResponse = await _client.PostAsJsonAsync("/TennisPlayers", newPlayer);
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
            var updateResponse = await _client.PutAsJsonAsync($"/TennisPlayers/{createdPlayer!.Id}", updated);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var player = await updateResponse.Content.ReadFromJsonAsync<Player>();
            Assert.Equal("Rafael", player!.FirstName);
            Assert.Equal(1, player.Data.Rank);
        }

        [Fact]
        public async Task DeletePlayer_ReturnsNoContent()
        {
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
            var createResponse = await _client.PostAsJsonAsync("/TennisPlayers", newPlayer);
            var createdPlayer = await createResponse.Content.ReadFromJsonAsync<Player>();

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/TennisPlayers/{createdPlayer!.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // GET should return 404
            var getResponse = await _client.GetAsync($"/TennisPlayers/{createdPlayer.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task GetStatistics_ReturnsOk()
        {
            var response = await _client.GetAsync("/TennisPlayers/statistics");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var stats = await response.Content.ReadFromJsonAsync<StatisticsResult>();
            Assert.NotNull(stats);
        }
    }
}