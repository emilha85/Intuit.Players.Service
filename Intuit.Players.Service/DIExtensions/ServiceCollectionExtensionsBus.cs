using Intuit.Players.Bus;
using Intuit.Players.Models.Messages;

namespace Intuit.Players.Service.DIExtensions
{
    public static class ServiceCollectionExtensionsBus
    {
        public static IServiceCollection AddBus(this IServiceCollection services)
        {
            return services.AddSingleton<IPlayersChannel<PlayersDataMessage>, UpdatePlayersDataChannel>()
                             .AddSingleton<IPlayersChannel<UpdatePlayerSearchedMessage>, UpdatePlayerSearchesChannel>();
        }
    }


}
