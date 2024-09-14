namespace Intuit.Players.Models;

public class EnrichedPlayer
{
    public EnrichedPlayer(Player player)
    {
        Player = player;
    }

    public Player Player { get; }

    public RetroSheetData RetroSheetData { get; set; }

    public BaseballReffernceData BaseballReffernceData { get; set; }

    public int NumberOfSearches { get; set; }
}