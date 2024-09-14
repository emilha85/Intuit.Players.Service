namespace Intuit.Players.Common.Options;

public class PlayersCacheOptions
{
    public const string SectionName = "PlayersCacheOptions";

    public int LruCacheCapacity { get; set; } = 100_000;
}
