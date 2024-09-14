using AsyncAwaitBestPractices;
using Intuit.Players.Bus;
using Intuit.Players.Extensions;
using Intuit.Players.Models.Interfaces;
using Intuit.Players.Models.Messages;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Intuit.Players.BL.Listeners;

public class PlayersDataMessageListener : IStartupTask
{
    private readonly IPlayersDal _playersDal;
    private readonly ILogger<PlayersDataMessageListener> _logger;
    private readonly ChannelReader<PlayersDataMessage> _reader;

    public PlayersDataMessageListener(
        IPlayersDal playersDal,
        IPlayersChannel<PlayersDataMessage> channel,
        ILogger<PlayersDataMessageListener> logger)
    {
        _playersDal = playersDal;
        _logger = logger;
        _reader = channel.Reader;
    }

    public Task Execute()
    {
        StartListener().SafeFireAndForget();

        return Task.CompletedTask;
    }

    public async Task StartListener()
    {
        while (await _reader.WaitToReadAsync())
        {
            await foreach (var message in _reader.ReadAllAsync())
            {
                OnMessageRecevied(message);
            }
        }
    }

    private void OnMessageRecevied(PlayersDataMessage message)
    {
        try
        {
            var players = message.Players.Select(p => p.ConvertToEnrichedPlayer()).ToList();

            // TODO - Check for deltas:
            // if new player-> add to db
            // if updated player-> run enrichment engine+ update db and cache(if exists in cache)                

            _playersDal.Upsert(players);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to handle incoming message");
        }
    }
}