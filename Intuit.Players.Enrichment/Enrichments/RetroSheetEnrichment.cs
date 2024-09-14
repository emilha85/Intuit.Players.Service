using HtmlAgilityPack;
using Intuit.Players.Common.Options;
using Intuit.Players.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;

namespace Intuit.Players.Enrichment.Enrichments;

public class RetroSheetEnrichment : IPlayerEnrichment
{
    private const string UrlFormat = @"https://www.retrosheet.org/boxesetc/{0}/P{1}.htm";
    private readonly ILogger<RetroSheetEnrichment> _logger;
    private SemaphoreSlim _semaphore;

    public RetroSheetEnrichment(IOptions<ScraperEnrichmentOptions> options, ILogger<RetroSheetEnrichment> logger)
    {
        _logger = logger;
        _semaphore = new SemaphoreSlim(options.Value.ParrallelisimDegree);
    }

    public async Task EnrichAsync(EnrichedPlayer playerEnriched)
    {
        await _semaphore.WaitAsync();

        try
        {
            var id = playerEnriched.Player.RetroSheetId;
            string firstChar = id.Substring(0, 1).ToUpper();
            var url = string.Format(UrlFormat, firstChar, id);

            try
            {
                using var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var preNode = htmlDoc.DocumentNode.SelectSingleNode("//pre[6]");

                if (preNode != null)
                {
                    playerEnriched.RetroSheetData = new RetroSheetData { TranasctionInfo = preNode.InnerText };
                    return;
                }

                _logger.LogError("Tranasction info node is not found");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error, failed to get TranasctionInfo data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error: failed to get TranasctionInfo");
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public bool IsValid(EnrichedPlayer playerEnriched)
    {
        return !string.IsNullOrEmpty(playerEnriched.Player.RetroSheetId) &&
            playerEnriched.RetroSheetData is null;
    }
}
