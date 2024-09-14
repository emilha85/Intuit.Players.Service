using AsyncAwaitBestPractices;
using Intuit.Players.Bus;
using Intuit.Players.Models.Interfaces;
using Intuit.Players.Models.Messages;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Intuit.Players.BL.Listeners;

public class PlayersSearchedMessageListener : IStartupTask
{
    private readonly IPlayersDal _playersDal;
    private readonly ILogger<PlayersSearchedMessageListener> _logger;

    private ChannelReader<UpdatePlayerSearchedMessage> _reader;

    public PlayersSearchedMessageListener(IPlayersDal playersDal, IPlayersChannel<UpdatePlayerSearchedMessage> channel, ILogger<PlayersSearchedMessageListener> logger)
    {
        _playersDal = playersDal;
        _reader = channel.Reader;
        _logger = logger;
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
            await foreach (var item in _reader.ReadAllAsync())
            {
                try
                {
                    _playersDal.IncrementNumberOfSearches(item.PlayerId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send increment message");                        
                }
            }
        }
    }
}