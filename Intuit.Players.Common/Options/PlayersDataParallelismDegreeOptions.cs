namespace Intuit.Players.Common.Options;

public class PlayersDataParallelismDegreeOptions
{
    public const string SectionName = "PlayersDataParallelismDegreeOptions";

    public int ParallelismDegree { get; set; } = 1_000;
}
