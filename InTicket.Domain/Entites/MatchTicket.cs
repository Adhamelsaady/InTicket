namespace InTicket.Domain;

public class MatchTicket : Ticket
{
    public Guid MatchId { get; set; }
    public Match Match { get; set; } = null!;
    
    public bool HomeTeamTicket {get; set;}
    
    public MatchTicketClass TicketClass { get; set; }
}

public enum MatchTicketClass
{
    ThirdClass_Left,
    ThirdClass_Right,
    SecondClass_Left,
    SecondClass_Right,
    FirstClass_Left,
    FirstClass_Right,
    VIP
}