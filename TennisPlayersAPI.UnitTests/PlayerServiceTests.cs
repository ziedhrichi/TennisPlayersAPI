using Moq;
using System.Numerics;
using System.Text.Json;
using TennisPlayersAPI.Data;
using TennisPlayersAPI.Exceptions;
using TennisPlayersAPI.Models;
using TennisPlayersAPI.Repositories;
using TennisPlayersAPI.Services;

namespace TennisPlayersAPI.UnitTests
{
    /// <summary>
    /// Class des tests unitaire de la partie logique métier 
    /// </summary>
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _mockRepo;
        private readonly PlayersService _service;

        public PlayerServiceTests()
        {
            _mockRepo = new Mock<IPlayerRepository>();
            _service = new PlayersService(_mockRepo.Object);
        }

        [Fact]
        public void GetAllPlayers_ShouldReturnOrderedPlayers()
        {
            // Arrange
            var players = new List<Player>
        {
            new() { Id = 1, FirstName = "Roger", LastName = "Federer", Sex ="Homme", Country=new(){ Code="ESP", Picture="Ftp://123.jpg"}, Data = new PlayerData { Rank = 3 } },
            new() { Id = 2, FirstName = "Rafael", LastName = "Nadal", Sex ="Homme", Country=new(){ Code="ESP", Picture="Ftp://123.jpg"}, Data = new PlayerData { Rank = 1 } },
            new() { Id = 3, FirstName = "Hrichi", LastName = "Zied", Sex ="Homme", Country=new(){ Code="TUN", Picture="Ftp://123.jpg"},Data = new PlayerData { Rank = 2 } }
        };

            _mockRepo.Setup(r => r.GetAll()).Returns(players);

            // Act
            var result = _service.GetAllPlayers().ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result.First().Data.Rank); // doit être trié
        }

        [Fact]
        public void GetAllPlayers_ShouldThrow_WhenNoPlayers()
        {
            _mockRepo.Setup(r => r.GetAll()).Returns(new List<Player>());

            Assert.Throws<PlayerException>(() => _service.GetAllPlayers());
        }

        [Fact]
        public void GetPlayerById_ShouldReturnPlayer()
        {
            var player = new Player { Id = 1, FirstName = "Roger", LastName = "Federer", Sex = "Homme", Country = new() { Code = "ESP", Picture = "Ftp://123.jpg" }, Data = new PlayerData { Rank = 3 } };
            _mockRepo.Setup(r => r.GetById(1)).Returns(player);

            var result = _service.GetPlayerById(1);

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetPlayerById_ShouldThrow_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Player)null);

            Assert.Throws<PlayerException>(() => _service.GetPlayerById(99));
        }

        [Fact]
        public void AddPlayer_ShouldAddNewPlayer()
        {
            var player = new Player { Id = 10, FirstName = "Roger", LastName = "Federer", Sex = "Homme", Country = new() { Code = "ESP", Picture = "Ftp://123.jpg" }, Data = new PlayerData { Rank = 3 } };

            _mockRepo.Setup(r => r.GetAll()).Returns(new List<Player>());
            _mockRepo.Setup(r => r.Add(It.IsAny<Player>())).Returns(player);

            var result = _service.AddPlayer(player);

            Assert.Equal(10, result.Id);
            _mockRepo.Verify(r => r.Add(player), Times.Once);
        }

        [Fact]
        public void AddPlayer_ShouldThrow_WhenPlayerAlreadyExists()
        {
            var player = new Player { Id = 1, FirstName = "Roger", LastName = "Federer", Sex = "Homme", Country = new() { Code = "ESP", Picture = "Ftp://123.jpg" }, Data = new PlayerData { Rank = 3 } };

            _mockRepo.Setup(r => r.GetAll()).Returns(new List<Player> { player });

            Assert.Throws<PlayerException>(() => _service.AddPlayer(player));
        }

        [Fact]
        public void UpdatePlayer_ShouldUpdateAndReturnNewPlayer()
        {
            var existing = new Player { Id = 1, FirstName = "Roger", LastName = "Federer", Sex = "Homme", Country = new() { Code = "ESP", Picture = "Ftp://123.jpg" }, Data = new PlayerData { Rank = 3 } };
            var updated = new Player { Id = 1, FirstName = "Roger", LastName = "Fred", Sex = "Homme", Country = new() { Code = "ESP", Picture = "Ftp://123.jpg" }, Data = new PlayerData { Rank = 5} };

            _mockRepo.Setup(r => r.GetById(1)).Returns(existing);
            _mockRepo.Setup(r => r.Update(1, updated)).Returns(updated);

            var result = _service.UpdatePlayer(1, updated);

            Assert.Equal(5, result.Data.Rank);
        }

        [Fact]
        public void UpdatePlayer_ShouldThrow_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Player)null);

            Assert.Throws<PlayerException>(() => _service.UpdatePlayer(1, new Player { Id = 1, FirstName = "Roger", LastName = "Fred", Sex = "Homme", Country = new() { Code = "ESP", Picture = "Ftp://123.jpg" }, Data = new PlayerData { Rank = 5 } }));
        }

        [Fact]
        public void DeletePlayer_ShouldReturnTrue_WhenDeleted()
        {
            var player = new Player { Id = 1, FirstName = "Roger", LastName = "Federer", Sex = "Homme", Country = new() { Code = "ESP", Picture = "Ftp://123.jpg" }, Data = new PlayerData { Rank = 3 } };
            _mockRepo.Setup(r => r.GetById(1)).Returns(player);
            _mockRepo.Setup(r => r.Delete(1)).Returns(true);

            var result = _service.DeletePlayer(1);

            Assert.True(result);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }

        [Fact]
        public void DeletePlayer_ShouldThrow_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Player)null);

            Assert.Throws<PlayerException>(() => _service.DeletePlayer(1));
        }

        [Fact]
        public void GetStats_ShouldReturnStatistics()
        {
            var players = new List<Player>
        {
            new Player {
                Id = 1,
                FirstName = "Roger", 
                LastName = "Federer", 
                Sex = "Homme", 
                Country = new Country { Code = "FR" },
                Data = new PlayerData { Weight = 80000, Height = 180, Last = new List<int> { 1, 0, 1 } }
            },
            new Player {
                Id = 2,
                FirstName = "Rafael",
                LastName = "Federer",
                Sex = "Homme",
                Country = new Country { Code = "US" },
                Data = new PlayerData { Weight = 90000, Height = 190, Last = new List<int> { 0, 0, 1 } }
            }
        };

            _mockRepo.Setup(r => r.GetAll()).Returns(players);

            var result = _service.GetStats();

            Assert.NotNull(result);
            Assert.True(result.AverageBmi > 0);
            Assert.True(result.MedianHeight > 0);
            Assert.False(string.IsNullOrEmpty(result.BestCountry));
        }
    }
}