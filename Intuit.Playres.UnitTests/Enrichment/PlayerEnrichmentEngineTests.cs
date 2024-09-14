using Intuit.Players.Enrichment;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Intuit.Players.Models;

namespace Intuit.Playres.UnitTests.Enrichment
{
    public class PlayerEnrichmentEngineTests
    {
        private Mock<IPlayerEnrichment> _enrichment1 = new Mock<IPlayerEnrichment>();
        private Mock<IPlayerEnrichment> _enrichment2 = new Mock<IPlayerEnrichment>();

        private PlayerEnrichmentEngine _target;

        [TestInitialize]
        public void Init()
        {
            _target = new PlayerEnrichmentEngine([_enrichment1.Object, _enrichment2.Object], NullLogger<PlayerEnrichmentEngine>.Instance);
        }

        [TestMethod]
        public void Enrich_ExecutedAllEnrichments_Success()
        {
            // Arrange
            _enrichment1.Setup(x => x.IsValid(It.IsAny<EnrichedPlayer>())).Returns(true);
            _enrichment2.Setup(x => x.IsValid(It.IsAny<EnrichedPlayer>())).Returns(true);

            // Act
            var player = new Players.Models.Player();
            var enrichmedPlayer = _target.Enrich(new EnrichedPlayer(player));

            // Assert
            _enrichment1.Verify(x => x.EnrichAsync(It.IsAny<EnrichedPlayer>()), Times.Once);
            _enrichment2.Verify(x => x.EnrichAsync(It.IsAny<EnrichedPlayer>()), Times.Once);
        }
    }
}
