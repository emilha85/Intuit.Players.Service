using Intuit.Players.Common.Options;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Intuit.Players.Common;

public class EnrichmentPlayersDataSemaphoreSet
{
    private readonly ConcurrentDictionary<int, SemaphoreSlim> _semaphoreSlimSet;
    private readonly PlayersDataParallelismDegreeOptions _parallelismDegreeOptions;

    public EnrichmentPlayersDataSemaphoreSet(IOptions<PlayersDataParallelismDegreeOptions> options)
    {
        _parallelismDegreeOptions = options.Value;
        _semaphoreSlimSet = new ConcurrentDictionary<int, SemaphoreSlim>(concurrencyLevel: -1, capacity: _parallelismDegreeOptions.ParallelismDegree);
    }

    public async Task<SemaphoreSlim> AcquireLock(string playerId)
    {
        var key = BuildKey(playerId);
        var semaphore = _semaphoreSlimSet.GetOrAdd(key, (k) => new SemaphoreSlim(1));

        await semaphore.WaitAsync();

        return semaphore;
    }

    private int BuildKey(string playerId)
    {
        int hash = playerId.GetHashCode() % _parallelismDegreeOptions.ParallelismDegree;
        return hash;
    }
}
