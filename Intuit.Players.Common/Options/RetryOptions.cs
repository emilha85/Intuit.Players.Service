namespace Intuit.Players.Common.Options
{
    public class RetryOptions
    {
        public const string SectionName = "RetryOptions";

        public int MaxRetryAttempts { get; set; } = 5;
    }
}
