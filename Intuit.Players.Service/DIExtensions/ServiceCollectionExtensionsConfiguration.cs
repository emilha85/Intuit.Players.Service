using Intuit.Players.Common.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Intuit.Players.Service.DIExtensions
{
    public static class ServiceCollectionExtensionsConfiguration
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                 .Configure<PlayersCacheOptions>(configuration.GetSection(PlayersCacheOptions.SectionName))
                 .Configure<PlayersDataParallelismDegreeOptions>(configuration.GetSection(PlayersDataParallelismDegreeOptions.SectionName))
                 .Configure<CsvPlayersReaderOptions>(configuration.GetSection(CsvPlayersReaderOptions.SectionName))
                 .Configure<ScraperEnrichmentOptions>(configuration.GetSection(ScraperEnrichmentOptions.SectionName))
                 .Configure<RetryOptions>(configuration.GetSection(RetryOptions.SectionName))
                 .Configure<UpdatePlayerDataJobOptions>(configuration.GetSection(UpdatePlayerDataJobOptions.SectionName));                 
        }

    }


}
