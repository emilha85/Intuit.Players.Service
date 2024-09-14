using Intuit.Players.Models;

namespace Intuit.Players.Extensions;

public static class PlayersMappingExtensions
{
    public static EnrichedPlayer ConvertToEnrichedPlayer(this PlayerDto dto)
    {
        if (dto is null)
        {
            return null;
        }

        var player = new Player
        {
            Bats = dto.bats,
            BaseballRefId = dto.bbrefID,
            BirthCity = dto.birthCity,
            BirthCountry = dto.birthCountry,
            BirthState = dto.birthState,
            BirthDay = dto.birthDay,
            BirthMonth = dto.birthMonth,
            BirthYear = dto.birthYear,
            Debut = dto.debut,
            DeathCity = dto.deathCity,
            DeathCountry = dto.deathCountry,
            DeathDay = dto.deathDay,
            DeathMonth = dto.deathMonth,
            DeathYear = dto.deathYear,
            DeceasedState = dto.deathState,
            FinalGame = dto.finalGame,
            FirstName = dto.nameFirst,
            GivenName = dto.nameGiven,
            Height = dto.height,
            Id = dto.playerID,
            LastName = dto.nameLast,
            RetroSheetId = dto.retroID,
            Throws = dto.throws,
            Weight = dto.weight
        };

        return new EnrichedPlayer(player);
    }
}
