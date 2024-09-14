using Intuit.Players.Models;
using Microsoft.Extensions.Logging;

namespace Intuit.Players.Enrichment;

public class PlayerEnrichmentEngine : IPlayerEnrichmentEngine
{
    private readonly IEnumerable<IPlayerEnrichment> _enrichments;
    private readonly ILogger<PlayerEnrichmentEngine> _logger;

    public PlayerEnrichmentEngine(IEnumerable<IPlayerEnrichment> enrichments, ILogger<PlayerEnrichmentEngine> logger)
    {
        _enrichments = enrichments;
        _logger = logger;
    }

    public async Task Enrich(EnrichedPlayer enrichedPlayer)
    {
        _logger.LogInformation($"Starting enrichment for Player: {enrichedPlayer.Player.Id}");

        var enrichmentTasks = new List<Task>();

        foreach (var enrichment in _enrichments)
        {
            if (enrichment.IsValid(enrichedPlayer))
            {
                var enrichmentTask = enrichment.EnrichAsync(enrichedPlayer);
                enrichmentTasks.Add(enrichmentTask);
            }
        }

        await Task.WhenAll(enrichmentTasks);

        _logger.LogInformation($"Done enrichment for Player: {enrichedPlayer.Player.Id}");
    }
}
