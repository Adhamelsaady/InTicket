namespace InTicket.Domain;

public class BaseEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    
    public string Location { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public int Price { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableTickets { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}