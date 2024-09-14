namespace Intuit.Players.Common.Options;

public class CsvPlayersReaderOptions
{
    public const string SectionName = "CsvPlayersReaderOptions";

    public int MaxChunkSize { get; set; } = 50_000;
}
