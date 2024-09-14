using Intuit.Players.Dal;
using Intuit.Players.Models.Interfaces;

namespace Intuit.Players.Service.DIExtensions
{
    public static class ServiceCollectionExtensionsDal
    {
        public static IServiceCollection AddDal(this IServiceCollection services)
        {
            return services.AddSingleton<IPlayersDal, PlayersDal>();
        }
    }


}
