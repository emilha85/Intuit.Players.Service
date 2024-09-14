using Intuit.Players.Models.Interfaces;
using System.Collections.Concurrent;
using Intuit.Players.Models;

namespace Intuit.Players.Dal;

public class PlayersDal : IPlayersDal
{
    private readonly ConcurrentDictionary<string, EnrichedPlayer> _players;

    public PlayersDal()
    {
        _players = new ConcurrentDictionary<string, EnrichedPlayer>();
    }

    public void Upsert(IReadOnlyList<EnrichedPlayer> enrichedPlayers)
    {
        Parallel.ForEach(enrichedPlayers, enrichedPlayer =>
        {
            if (string.IsNullOrEmpty(enrichedPlayer?.Player?.Id))
            {
                return;
            }

            _players.AddOrUpdate(enrichedPlayer.Player.Id, enrichedPlayer, (key, val) => val);
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


}
