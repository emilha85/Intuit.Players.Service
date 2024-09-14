using Intuit.Players.Bus;
using Intuit.Players.Common;
using Intuit.Players.Enrichment;
using Intuit.Players.Models;
using Intuit.Players.Models.Interfaces;
using Intuit.Players.Models.Messages;
using System.Threading.Channels;

namespace Intuit.Players.BL;

public class PlayersHandler : IPlayersHandler
{
    private readonly IPlayersDal _playersDal;
    private readonly IPlayersCache _playersCache;
    private readonly IPlayerEnrichmentEngine _enrichmentEngine;
    private readonly ChannelWriter<UpdatePlayerSearchedMessage> _updatePlayerSearchesProducer;
    private readonly EnrichmentPlayersDataSemaphoreSet _playersSemaphoreSet;

    public PlayersHandler(
        IPlayersDal playersDal,
        IPlayersCache playersCache,
        IPlayerEnrichmentEngine enrichmentEngine,
        IPlayersChannel<UpdatePlayerSearchedMessage> channel,
        EnrichmentPlayersDataSemaphoreSet playersSemaphoreSet)
    {
        _playersDal = playersDal;
        _playersCache = playersCache;
        _enrichmentEngine = enrichmentEngine;
        _updatePlayerSearchesProducer = channel.Writer;
        _playersSemaphoreSet = playersSemaphoreSet;
    }

    public IReadOnlyList<Player> GetAllPlayers(int limit, int offset)
    {
        var players = _playersDal.GetAllPlayers(limit, offset);
        return players.Select(p => p.Player).ToList();
    }

    public async Task<EnrichedPlayer> GetById(string playerId)
    {
        var enrichedPlayer = _playersCache.Get(playerId);
        if (enrichedPlayer is null)
        {
            var release = await _playersSemaphoreSet.AcquireLock(playerId);
            try
            {
                enrichedPlayer = _playersCache.Get(playerId);
                if (enrichedPlayer is null)
                {
                    enrichedPlayer = await EnrichPlayer(playerId);
                    UpdateDbs(enrichedPlayer); 
                }
            }
            finally
            {
                release.Release();
            }
        }

        await NotifyPlayerSearched(playerId);

        return enrichedPlayer;
    }

    private void UpdateDbs(EnrichedPlayer enrichedPlayer)
    {
        _playersCache.Upsert([enrichedPlayer]);
        _playersDal.Upsert([enrichedPlayer]);
    }

    private async Task<EnrichedPlayer> EnrichPlayer(string playerId)
    {
        var player = _playersDal.GetPlayer(playerId);
        if (player is null)
        {
            throw new NotFoundException($"player with id: {playerId} not found");
        }

        await _enrichmentEngine.Enrich(player);

        return player;
    }    

    private async Task NotifyPlayerSearched(string playerId)
    {
        var message = new UpdatePlayerSearchedMessage { PlayerId = playerId };
        await _updatePlayerSearchesProducer.WriteAsync(message);
    }
}
