using Intuit.Players.Dal;
using Intuit.Players.Models;

namespace Intuit.Playres.UnitTests.Dal
{
    [TestClass]
    public class PlayersDalTests
    {
        private PlayersDal _target;

        [TestInitialize]
        public void Init()
        {
            _target = new PlayersDal();
        }

        [TestMethod]
        public void Upsert_PlayerIsNull_DbNotUpdated()
        {
            // Arrange
            Player player = null;

            // Act
            _target.Upsert([new EnrichedPlayer(player)]);

            // Assert
            Assert.AreEqual(0, _target.GetAllPlayers(1, 1).Count);
        }

        [TestMethod]
        public void Upsert()
        {
            // Arrange
            var players = new List<EnrichedPlayer>
            {
                new EnrichedPlayer(new Player { Id = "1" } ),
                new EnrichedPlayer( new Player { Id = "2" } ),
            };

            // Act
            _target.Upsert(players);

            // Assert
            for (int i = 0; i < players.Count; i++)
            {
                var expectedPlayer = players[i];
                var actualPlayer = _target.GetPlayer(expectedPlayer.Player.Id);

                Assert.IsNotNull(actualPlayer);
                Assert.AreEqual(expectedPlayer.Player.Id, actualPlayer.Player.Id);
            }
        }

        [TestMethod]
        public void GetAllPlayers()
        {
            // Arrange
            var players = new List<EnrichedPlayer>
            {
                new EnrichedPlayer(new Player { Id = "1"} ),
                new EnrichedPlayer( new Player { Id = "2"} )
            };

            _target.Upsert(players);

            // Act
            var res = _target.GetAllPlayers(1, 1);

            // Assert
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual(players[1].Player.Id, res[0].Player.Id);

        }

        [TestMethod]
        public void GetPlayer()
        {
            // Arrange
            var player = new EnrichedPlayer(new Player { Id = "1" });

            _target.Upsert([player]);

            // Act
            var res = _target.GetPlayer(player.Player.Id);

            // Assert
            Assert.AreEqual(player.Player.Id, res.Player.Id);
        }

        [TestMethod]
        public void GetPlayer_NotExists()
        {
            // Act
            var res = _target.GetPlayer("1");

            // Assert
            Assert.IsNull(res);
        }

        [TestMethod]
        public void GetTopSearcedPlayers()
        {
            // Arrange
            var players = new List<EnrichedPlayer>
            {
                new EnrichedPlayer(new Player { Id = "1" })
                {
                    NumberOfSearches = 10
                },
                new EnrichedPlayer(new Player { Id = "2" })
                {
                    NumberOfSearches = 9
                },
                new EnrichedPlayer(new Player { Id = "3" })
                {
                    NumberOfSearches = 1
                },
            };

            _target.Upsert(players);

            // Act
            var res = _target.GetTopSearchedPlayers(2);

            // Assert
            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("1", res[0].Player.Id);
            Assert.AreEqual("2", res[1].Player.Id);
        }

        [TestMethod]
        public void IncrementNumberOfSearches()
        {
            // Arrange
            var players = new List<EnrichedPlayer>
            {
                new EnrichedPlayer(new Player { Id = "1" })
                {
                    NumberOfSearches = 1
                },
            };

            _target.Upsert(players);

            // Act
            _target.IncrementNumberOfSearches(players[0].Player.Id);

            // Assert
            var actualPlayer = _target.GetPlayer(players[0].Player.Id);
            Assert.AreEqual(2, actualPlayer.NumberOfSearches);
        }
    }
}
