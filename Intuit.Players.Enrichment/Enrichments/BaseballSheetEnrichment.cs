using HtmlAgilityPack;
using Intuit.Players.Common.Options;
using Intuit.Players.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Intuit.Players.Enrichment.Enrichments;

public class BaseballSheetEnrichment : IPlayerEnrichment
{
    private const string UrlFormat = @"https://www.baseball-reference.com/players/{0}/{1}.shtml";
    private readonly IOptions<ScraperEnrichmentOptions> options;
    private readonly ILogger<BaseballSheetEnrichment> _logger;

    private SemaphoreSlim _semaphore;

    public BaseballSheetEnrichment(IOptions<ScraperEnrichmentOptions> options, ILogger<BaseballSheetEnrichment> logger)
    {
        this.options = options;
        _logger = logger;
        _semaphore = new SemaphoreSlim(options.Value.ParrallelisimDegree);
    }

    public async Task EnrichAsync(EnrichedPlayer playerEnriched)
    {
        await _semaphore.WaitAsync();

        try
        {
            

            var id = playerEnriched.Player.BaseballRefId;
            string firstChar = id.Substring(0, 1);

            var url = string.Format(UrlFormat, firstChar, id);

            try
            {
                using var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var preNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='meta']/div[2]/p[7]");

                if (preNode != null)
                {
                    playerEnriched.BaseballReffernceData = new BaseballReffernceData { EducationData = preNode.InnerText };
                    return;
                }

                _logger.LogError("The target pre node could not be found");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error, failed to get EducationData");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error: failed to get EducationData");
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public bool IsValid(EnrichedPlayer playerEnriched)
    {
        return !string.IsNullOrEmpty(playerEnriched.Player.BaseballRefId) &&
            playerEnriched.BaseballReffernceData is null;
    }
}