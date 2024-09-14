using Intuit.Players.Models.Interfaces;
using System.Collections.Concurrent;
using Intuit.Players.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Intuit.Players.Common.Options;

namespace Intuit.Players.Dal;

public class PlayersDal : IPlayersDal
{
    private readonly ConcurrentDictionary<string, EnrichedPlayer> _players;
    private readonly RetryOptions _options;
    private readonly ILogger<PlayersDal> _logger;    

    public PlayersDal(IOptions<RetryOptions> options, ILogger<PlayersDal> logger)
    {
        _players = new ConcurrentDictionary<string, EnrichedPlayer>();
        _options = options.Value;
        _logger = logger;
    }

    public void Upsert(IReadOnlyList<EnrichedPlayer> enrichedPlayers)
    {
        Parallel.ForEach(enrichedPlayers, enrichedPlayer =>
        {
            if (string.IsNullOrEmpty(enrichedPlayer?.Player?.Id))
            {
                return;
            }

            ExecuteWithRetry(() => _players.AddOrUpdate(enrichedPlayer.Player.Id, enrichedPlayer, (key, val) => val));

        });
    }

    public IReadOnlyList<EnrichedPlayer> GetAllPlayers(int limit, int offset)
    {
        var players = _players.Values.Skip(offset * limit).
                                      Take(limit).
                                      ToList();

        return players;
    }

    public EnrichedPlayer GetPlayer(string playerId)
    {
        _players.TryGetValue(playerId, out var player);

        if (player is null)
        {
            return null;
        }

        return player;
    }

    public IReadOnlyList<EnrichedPlayer> GetTopSearchedPlayers(int top)
    {
        return _players.Values
            .OrderByDescending(x => x.NumberOfSearches)
            .Take(top)
            .ToList();
    }

    public void IncrementNumberOfSearches(string playerId)
    {
        if (!_players.TryGetValue(playerId, out var player))
        {
            return;
        }

        player.NumberOfSearches++;
    }

    private void ExecuteWithRetry(Func<EnrichedPlayer> action)
    {
        var retryCount = 0;
        var passed = false;

        while(passed == false && retryCount < _options.MaxRetryAttempts)
        {
            try
            {
                action();
                passed = true;
            }
            catch (Exception)
            {
                retryCount++;                
            }
        }

        if(!passed)
        {
            _logger.LogError($"Failed to exceute operation after max: {_options.MaxRetryAttempts}");
        }
    }


}
