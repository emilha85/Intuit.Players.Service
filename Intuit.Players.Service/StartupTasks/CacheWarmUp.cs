using Intuit.Players.Cache;
using Intuit.Players.Common.Options;
using Intuit.Players.Models.Interfaces;
using Microsoft.Extensions.Options;

namespace Intuit.Players.Service.StartupTasks
{
    public class CacheWarmUp
    {
        private readonly IPlayersDal _playersDal;
        private readonly IPlayersCache _playersCache;
        private readonly IOptions<PlayersCacheOptions> _options;

        public CacheWarmUp(IPlayersDal playersDal, IPlayersCache playersCache, IOptions<PlayersCacheOptions> options)
        {
            _playersDal = playersDal;
            _playersCache = playersCache;
            _options = options;            
        }

        public void Execute()
        {
            var topSearchedPlayers = _playersDal.GetTopSearchedPlayers(_options.Value.LruCacheCapacity);
            _playersCache.Upsert(topSearchedPlayers);
        }
    }
}
