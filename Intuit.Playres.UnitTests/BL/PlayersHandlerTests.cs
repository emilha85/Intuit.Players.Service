using Intuit.Players.BL;
using Intuit.Players.Bus;
using Intuit.Players.Common;
using Intuit.Players.Common.Options;
using Intuit.Players.Enrichment;
using Intuit.Players.Models;
using Intuit.Players.Models.Interfaces;
using Microsoft.Extensions.Options;
using Moq;

namespace Intuit.Playres.UnitTests.BL
{
    [TestClass]
    public class PlayersHandlerTests
    {
        private PlayersHandler _target;

        private Mock<IPlayersDal> _playersDal = new Mock<IPlayersDal>();
        private Mock<IPlayersCache> _playersCache = new Mock<IPlayersCache>();
        private Mock<IPlayerEnrichmentEngine> _enrichmentEngine = new Mock<IPlayerEnrichmentEngine>();

        [TestInitialize]
        public void Init()
        {
            var options = Options.Create(new PlayersDataParallelismDegreeOptions());

            _target = new PlayersHandler(
                _playersDal.Object,
                _playersCache.Object,
                _enrichmentEngine.Object,
                new UpdatePlayerSearchesChannel(),
                new EnrichmentPlayersDataSemaphoreSet(options));
        }

        [TestMethod]
        public void GetAllPlayers_Success()
        {
            // Arrange
            var limit = 10;
            var offset = 1;

            var player = new EnrichedPlayer(new Player { Id = "1" });

            _playersDal.Setup(x => x.GetAllPlayers(limit, offset))
                .Returns([player]);

            // Act
            var players = _target.GetAllPlayers(10, 1);

            // Assert
            Assert.AreEqual(1, players.Count);
            Assert.AreEqual("1", players[0].Id);
        }

        [TestMethod]
        public async Task GetById_InCache_NoEnrichment()
        {
            // Arrange
            var enrichedPlayer = new EnrichedPlayer(new Player { Id = "1" });

            _playersCache.Setup(x => x.Get(enrichedPlayer.Player.Id))
                .Returns(enrichedPlayer);

            // Act
            var actualPlayer = await _target.GetById(enrichedPlayer.Player.Id);

            // Assert
            Assert.IsNotNull(actualPlayer);
            Assert.AreEqual(enrichedPlayer.Player.Id, actualPlayer.Player.Id);

            _playersCache.Verify(x => x.Get(enrichedPlayer.Player.Id), Times.Once);
            _enrichmentEngine.Verify(x => x.Enrich(It.IsAny<EnrichedPlayer>()), Times.Never);
        }

        [TestMethod]
        public async Task GetById_NotInCache_NotInDb_Throws()
        {
            // Arrange
            var limit = 10;
            var offset = 1;

            var enrichedPlayer = new EnrichedPlayer(new Player { Id = "1" });

            _playersCache.Setup(x => x.Get(enrichedPlayer.Player.Id))
                .Returns((EnrichedPlayer)null);

            _playersDal.Setup(x => x.GetPlayer(enrichedPlayer.Player.Id))
                .Returns((EnrichedPlayer)null);

            // Act + Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _target.GetById(enrichedPlayer.Player.Id));
        }


        [TestMethod]
        public async Task GetById_NotInCache_ExistsInDb_PerformEnrichment()
        {
            // Arrange
            var limit = 10;
            var offset = 1;

            var enrichedPlayer = new EnrichedPlayer(new Player { Id = "1" });

            _playersCache.Setup(x => x.Get(enrichedPlayer.Player.Id))
                .Returns((EnrichedPlayer)null);

            _playersDal.Setup(x => x.GetPlayer(enrichedPlayer.Player.Id))
                .Returns(enrichedPlayer);

            // Act
            var actualPlayer = await _target.GetById(enrichedPlayer.Player.Id);

            // Assert
            Assert.IsNotNull(actualPlayer);
            _enrichmentEngine.Verify(x => x.Enrich(It.IsAny<EnrichedPlayer>()), Times.Once);
            _playersCache.Verify(x => x.Upsert(It.IsAny<IReadOnlyList<EnrichedPlayer>>()), Times.Once);
            _playersDal.Verify(x => x.Upsert(It.IsAny<IReadOnlyList<EnrichedPlayer>>()), Times.Once);
        }
    }
}
