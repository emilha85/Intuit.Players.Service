namespace Intuit.Players.Models.Interfaces;

public interface IPlayersCache
{
    void Upsert(IReadOnlyList<EnrichedPlayer> players);

    EnrichedPlayer Get(string playerId);

    void Remove(string playerId);

    int GetPlayersCount();
}