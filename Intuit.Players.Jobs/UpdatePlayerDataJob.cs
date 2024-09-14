using Intuit.Players.Common.Options;
using Intuit.Players.Models.Interfaces;
using Microsoft.Extensions.Options;
using System.Timers;

namespace Intuit.Players.Jobs
{
    public class UpdatePlayerDataJob : IStartupTask
    {
        private readonly ICsvPlayersReader csvPlayersReader;        
        private readonly IOptions<UpdatePlayerDataJobOptions> options;

        public UpdatePlayerDataJob(
            ICsvPlayersReader csvPlayersReader,                       
            IOptions<UpdatePlayerDataJobOptions> options)
        {
            this.csvPlayersReader = csvPlayersReader;            
            this.options = options;
        }

        public Task Execute()
        {
            var timer = new System.Timers.Timer();
            timer.Elapsed += UpdateData;
            timer.Interval = options.Value.Refreshinterval.TotalMilliseconds;
            timer.Start();

            return Task.CompletedTask;
        }

        private void UpdateData(object? sender, ElapsedEventArgs e)
        {
            csvPlayersReader.LoadPlayersData();
        }
    }
}
