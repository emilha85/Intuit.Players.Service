using Intuit.Players.Cache;
using Intuit.Players.Common.Options;
using Intuit.Players.Models;
using Intuit.Players.Models.Interfaces;
using Microsoft.Extensions.Options;

namespace Intuit.Playres.UnitTests.Cache
{
    [TestClass]
    public class PlayersCacheTests
    {
        private const int CacheCapacity = 1000;

        private PlayersCache _target;

        [TestInitialize]
        public void Init()
        {
            _target = new PlayersCache(Options.Create(new PlayersCacheOptions { LruCacheCapacity = CacheCapacity }));
        }

        [TestMethod]
        public void Upsert_PlayerIsNull_CacheNotUpdated()
        {
            // Arrange
            Player player = null;

            // Act
            _target.Upsert([new EnrichedPlayer(player)]);

            // Assert
            Assert.AreEqual(0, _target.GetPlayersCount());
        }

        [TestMethod]
        public void Upsert()
        {
            // Arrange
            var players = new List<EnrichedPlayer>
            {
                new EnrichedPlayer(new Player { Id = "1"} ),
                new EnrichedPlayer( new Player { Id = "2"} ),
            };

            // Act
            _target.Upsert(players);

            // Assert
            for (int i = 0; i < players.Count; i++)
            {
                var expectedPlayer = players[i];
                var actualPlayer = _target.Get(expectedPlayer.Player.Id);

                Assert.IsNotNull(actualPlayer);
                Assert.AreEqual(expectedPlayer.Player.Id, actualPlayer.Player.Id);
            }
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            var player = new EnrichedPlayer(new Player { Id = "1" });

            _target.Upsert([player]);

            // Act
            _target.Remove(player.Player.Id);

            // Assert
            var actual = _target.Get(player.Player.Id);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Get_NotExists()
        {
            // Arrange
            var player = new EnrichedPlayer(new Player { Id = "1" });

            _target.Upsert([player]);

            // Act
            _target.Remove(player.Player.Id);

            // Assert
            var actual = _target.Get(player.Player.Id);

            Assert.IsNull(actual);
        }
    }
}
