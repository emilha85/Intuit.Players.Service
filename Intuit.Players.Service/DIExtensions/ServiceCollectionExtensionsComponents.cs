using Intuit.Players.BL;
using Intuit.Players.Common;
using Intuit.Players.Enrichment;
using Intuit.Players.Enrichment.Enrichments;
using Intuit.Players.Models.Interfaces;
using Intuit.Players.Service.StartupTasks;
using Intuit.Players.Utils;

namespace Intuit.Players.Service.DIExtensions
{
    public static class ServiceCollectionExtensionsComponents
    {
        public static IServiceCollection AddComponents(this IServiceCollection services)
        {
            return services.AddSingleton<IPlayersHandler, PlayersHandler>()
                            .AddSingleton<IPlayerEnrichmentEngine, PlayerEnrichmentEngine>()
                            .AddSingleton<CacheWarmUp>()
                            .AddSingleton<EnrichmentPlayersDataSemaphoreSet>()
                            .AddSingleton<IPlayerEnrichment, BaseballSheetEnrichment>()
                            .AddSingleton<IPlayerEnrichment, RetroSheetEnrichment>()
                            .AddSingleton<ICsvPlayersReader, CsvPlayersReader>();                            


        }
    }


}
