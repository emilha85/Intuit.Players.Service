using Intuit.Players.Models;

namespace Intuit.Players.Enrichment;

public interface IPlayerEnrichment
{
    bool IsValid(EnrichedPlayer playerEnriched);


    Task EnrichAsync(EnrichedPlayer playerEnriched);
}
