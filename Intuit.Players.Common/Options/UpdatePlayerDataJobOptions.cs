namespace Intuit.Players.Common.Options
{
    public class UpdatePlayerDataJobOptions
    {
        public const string SectionName = "UpdatePlayerDataJobOptions";

        public TimeSpan Refreshinterval { get; set; } = TimeSpan.FromHours(12);
    }
}
