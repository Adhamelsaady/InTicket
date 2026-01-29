namespace InTicket.Domain;

public class Match : BaseEvent
{
    public Guid HomeTeamId { get; set; }
    public Team HomeTeam { get; set; } = null!;
    
    public Guid AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = null!;
    
    public string League { get; set; } = string.Empty;  
    public string Season { get; set; } = string.Empty;  
    public int? Round { get; set; }  
    
    public string StadiumName { get; set; } = string.Empty;
    
    public int StadiumCapacity { get; set; }
    
    public DateTime? FanPriorityBookingStart { get; set; }  
    public DateTime? GeneralBookingStart { get; set; }       
    
    public ICollection <MatchTicket> MatchTickets { get; set; } = new List<MatchTicket>();
}