namespace InTicket.Domain;

public class Concert : BaseEvent
{
    public string Artist { get; set; }  
    public string? Organizer { get; set; }  
    
    public int? DurationMinutes { get; set; }
    public int? MinimumAge { get; set; }
    
    public ICollection <ConcertTicket>  ConcertTickets { get; set; } = new List<ConcertTicket>();
     
}