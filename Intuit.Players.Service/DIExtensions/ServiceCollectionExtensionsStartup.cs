using Intuit.Players.BL.Listeners;
using Intuit.Players.Jobs;
using Intuit.Players.Models.Interfaces;

namespace Intuit.Players.Service.DIExtensions
{
    public static class ServiceCollectionExtensionsStartup
    {
        public static IServiceCollection AddStartupTasks(this IServiceCollection services)
        {
            return services.AddSingleton<IStartupTask, PlayersSearchedMessageListener>()
                            .AddSingleton<IStartupTask, UpdatePlayerDataJob>()
                            .AddSingleton<IStartupTask, PlayersDataMessageListener>();
        }
    }


}
