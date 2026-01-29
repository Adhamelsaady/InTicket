namespace InTicket.Domain;

public class Ticket
{
    public Guid TicketId { get; set; } 
    public string? HolderId { get; set; } = string.Empty;
    public ApplicationUser? Holder { get; set; }
    public int Price { get; set; }
    public TicketStatus Status { get; set; } =  TicketStatus.Open;
    
    public DateTime? BookedAt { get; set; }
    public DateTime? HeldAt { get; set; }
    public DateTime? HeldExpiresAt { get; set; }
}

public enum  TicketStatus
{
    Open,
    Held,
    Booked
}
