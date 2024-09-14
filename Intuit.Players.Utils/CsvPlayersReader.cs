using CsvHelper;
using Intuit.Players.Bus;
using Intuit.Players.Common.Options;
using Intuit.Players.Models;
using Intuit.Players.Models.Interfaces;
using Intuit.Players.Models.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Threading.Channels;

namespace Intuit.Players.Utils;
public class CsvPlayersReader : ICsvPlayersReader
{
    private ChannelWriter<PlayersDataMessage> writer;
    private readonly IOptions<CsvPlayersReaderOptions> options;
    private readonly ILogger<CsvPlayersReader> _logger;

    public CsvPlayersReader(IPlayersChannel<PlayersDataMessage> channel, IOptions<CsvPlayersReaderOptions> options, ILogger<CsvPlayersReader> logger)
    {
        writer = channel.Writer;
        this.options = options;
        _logger = logger;
    }

    public async Task LoadPlayersData()
    {
        try
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExternalResources", "Players.csv");
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var innerList = new List<PlayerDto>();

                while (await csv.ReadAsync())
                {
                    var playerDto = csv.GetRecord<PlayerDto>();

                    innerList.Add(playerDto);
                    if (innerList.Count >= options.Value.MaxChunkSize)
                    {
                        await PublishMessage(innerList);
                        innerList.Clear();
                    }
                }

                // Hanlding remain players data
                if (innerList.Count > 0)
                {
                    await PublishMessage(innerList);
                    innerList.Clear();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load players data from csv");
        }
    }

    private async Task PublishMessage(List<PlayerDto> players)
    {
        var message = new PlayersDataMessage { Players = players.ToList() };

        await writer.WriteAsync(message);
    }
}
