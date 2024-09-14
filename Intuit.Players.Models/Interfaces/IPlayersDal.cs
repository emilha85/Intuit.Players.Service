namespace Intuit.Players.Models.Interfaces;

public interface IPlayersDal
{
    void Upsert(IReadOnlyList<EnrichedPlayer> players);    

    IReadOnlyList<EnrichedPlayer> GetAllPlayers(int limit, int offset);

    EnrichedPlayer GetPlayer(string playerId);

    IReadOnlyList<EnrichedPlayer> GetTopSearchedPlayers(int top);

    void IncrementNumberOfSearches(string playerId);
}