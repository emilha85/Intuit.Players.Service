using Intuit.Players.Common.Options;
using Intuit.Players.Models;
using Intuit.Players.Models.Interfaces;
using LruCacheNet;
using Microsoft.Extensions.Options;

namespace Intuit.Players.Cache;

public class PlayersCache : IPlayersCache
{
    private readonly LruCache<string, EnrichedPlayer> _lruCache;

    public PlayersCache(IOptions<PlayersCacheOptions> options)
    {
        _lruCache = new LruCache<string, EnrichedPlayer>(capacity: options.Value.LruCacheCapacity);
    }

    public void Upsert(IReadOnlyList<EnrichedPlayer> players)
    {
        Parallel.ForEach(players, p =>
        {
            if (string.IsNullOrEmpty(p.Player?.Id))
            {
                return;
            }

            _lruCache.AddOrUpdate(p.Player.Id, p);
        });
    }

    public EnrichedPlayer Get(string playerId)
    {
        _lruCache.TryGetValue(playerId, out EnrichedPlayer player);
        return player;
    }

    public int GetPlayersCount()
    {
        return _lruCache.Count;
    }

    public void Remove(string playerId)
    {
        _lruCache.Remove(playerId);
    }
}