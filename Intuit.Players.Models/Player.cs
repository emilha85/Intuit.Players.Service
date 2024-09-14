namespace Intuit.Players.Models;

public class Player
{
    public string Id { get; set; }

    public int? BirthYear { get; set; }

    public int? BirthMonth { get; set; }

    public int? BirthDay { get; set; }

    public string BirthCountry { get; set; }

    public string BirthState { get; set; }

    public string BirthCity { get; set; }

    public int? DeathYear { get; set; }

    public int? DeathMonth { get; set; }

    public int? DeathDay { get; set; }

    public string DeathCountry { get; set; }

    public string DeceasedState { get; set; }

    public string DeathCity { get; set; }

    public string FirstName { get; set; }

    public string GivenName { get; set; }

    public string LastName { get; set; }

    public int? Weight { get; set; }

    public int? Height { get; set; }

    public string Bats { get; set; }

    public string Throws { get; set; }

    public DateTime? Debut { get; set; }

    public DateTime? FinalGame { get; set; }

    public string RetroSheetId { get; set; }

    public string BaseballRefId { get; set; }
}