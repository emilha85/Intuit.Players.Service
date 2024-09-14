using Intuit.Players.Cache;
using Intuit.Players.Models.Interfaces;

namespace Intuit.Players.Service.DIExtensions
{
    public static class ServiceCollectionExtensionsCache
    {
        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            return services.AddSingleton<IPlayersCache, PlayersCache>();
        }
    }


}
