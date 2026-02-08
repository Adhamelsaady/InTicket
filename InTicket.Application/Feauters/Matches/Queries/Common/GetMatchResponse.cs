using InTicket.Application.Feauters.Matchs.Queries;
namespace InTicket.Application.Feauters.Matchs.Queries.Common;
public class GetMatchResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    
    public string Location { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public TeamDto HomeTeam { get; set; } = null!;
    public TeamDto AwayTeam { get; set; } = null!;
    public string League { get; set; } = string.Empty;  
    public string Season { get; set; } = string.Empty;  
    public int? Round { get; set; }  
    
    public string StadiumName { get; set; } = string.Empty;
    public int StadiumCapacity { get; set; }
    public bool IsActive { get; set; }
}