namespace InTicket.Domain;

public class ConcertTicket : Ticket
{
    public Guid ConcertId  { get; set; } 
    public Concert Concert { get; set; } = null!;
    
    public ConcertTicketClass TicketClass { get; set; }
}

public enum ConcertTicketClass
{
    Regular , 
    VIP, 
    VVIP,
}