using Intuit.Players.Models;

namespace Intuit.Players.Enrichment
{
    public interface IPlayerEnrichmentEngine
    {
        Task Enrich(EnrichedPlayer enrichedPlayer);
    }
}