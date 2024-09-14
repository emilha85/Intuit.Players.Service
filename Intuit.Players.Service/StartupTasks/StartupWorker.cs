using Intuit.Players.Models.Interfaces;

namespace Intuit.Players.Service.StartupTasks
{
    public class StartupWorker : BackgroundService
    {
        private readonly IEnumerable<IStartupTask> _startupTasks;
        private readonly CacheWarmUp _cacheWarmUp;
        private readonly ICsvPlayersReader _csvPlayersReader;

        public StartupWorker(
            IEnumerable<IStartupTask> startupTasks,
            CacheWarmUp cacheWarmUp,
            ICsvPlayersReader csvPlayersReader)
        {
            _startupTasks = startupTasks;
            _cacheWarmUp = cacheWarmUp;
            _csvPlayersReader = csvPlayersReader;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tasks = new List<Task>(_startupTasks.Count());

            foreach (var task in _startupTasks)
            {
                var t = task.Execute();
                tasks.Add(t);
            }

            await Task.WhenAll(tasks);

            await _csvPlayersReader.LoadPlayersData();

            _cacheWarmUp.Execute();
        }
    }
}
