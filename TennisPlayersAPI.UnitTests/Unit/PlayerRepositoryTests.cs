using Moq;
using System.Numerics;
using System.Text.Json;
using TennisPlayers.Domain.Entities;
using TennisPlayers.Infrastructure.Persistence;

namespace TennisPlayers.Test.Unit
{
    /// <summary>
    /// Class des tests unitaires de la partie Accès aux données.
    /// </summary>
    public class PlayerRepositoryTests
    {
        private readonly Mock<IFileSystem> _fileSystemMock;
        private readonly PlayerRepository _repo;
        private readonly Root _rootData;

        public PlayerRepositoryTests()
        {
            _rootData = new Root
            {
                Players =
                [
                    new() { Id = 1, FirstName = "Roger", LastName = "Federer", Sex ="Homme", Country=new(){ Code="ESP", Picture="Ftp://123.jpg"}, Data=new(){ Age = 21} },
                    new() { Id = 2, FirstName = "Rafael", LastName = "Nadal", Sex ="Homme", Country=new(){ Code="ESP", Picture="Ftp://123.jpg"}, Data=new(){ Age = 21} }
                ]
            };

            var json = JsonSerializer.Serialize(_rootData);

            _fileSystemMock = new Mock<IFileSystem>();
            _fileSystemMock.Setup(fs => fs.ReadAllText(It.IsAny<string>())).Returns(json);

            _repo = new PlayerRepository(_fileSystemMock.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnPlayers()
        {
            var players = _repo.GetAllAsync();

            Assert.Equal(2, ((List<Player>)players).Count);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectPlayer()
        {
            var player = _repo.GetById(1);

            Assert.Equal("Roger", player.FirstName);
        }

        [Fact]
        public void Add_ShouldIncrementId_AndWriteFile()
        {
            var newPlayer = new Player { FirstName = "Novak", LastName = "Djokovic", Sex = "Homme", Country = new() { Code = "ESP", Picture = "Ftp://123.jpg" }, Data = new() { Age = 21 } };

            var added = _repo.Add(newPlayer);

            Assert.Equal(3, added.Id);
            _fileSystemMock.Verify(fs => fs.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldReplacePlayer_AndWriteFile()
        {
            var updated = new Player { FirstName = "Rafa", LastName = "Nadal", Sex = "Homme", Country = new() { Code = "ESP", Picture = "Ftp://123.jpg" }, Data = new() { Age = 21 } };

            var result = _repo.Update(2, updated);

            Assert.NotNull(result);
            Assert.Equal("Rafa", result.FirstName);
            _fileSystemMock.Verify(fs => fs.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldRemovePlayer_AndWriteFile()
        {
            var result = _repo.Delete(1);

            Assert.True(result);
            _fileSystemMock.Verify(fs => fs.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}