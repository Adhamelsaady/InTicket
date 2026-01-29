namespace InTicket.Domain;

public class BaseEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }

    public bool IsActive { get; set; } = false;
    
    public string Location { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
}