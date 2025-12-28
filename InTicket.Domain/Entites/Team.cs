namespace InTicket.Domain;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Stadium { get; set; }
    public string City { get; set; }
    
    public int FoundedYear { get; set; }

    public ICollection<ApplicationUser> Fans { get; set; } = new List<ApplicationUser>();
    public ICollection<Match> HomeMatches { get; set; } = new List<Match>();
    public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
}