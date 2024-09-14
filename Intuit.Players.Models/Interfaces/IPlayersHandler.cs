using Intuit.Players.Models;

namespace Intuit.Players.BL
{
    public interface IPlayersHandler
    {
        IReadOnlyList<Player> GetAllPlayers(int limit, int offset);

        Task<EnrichedPlayer> GetById(string playerId);
    }
}