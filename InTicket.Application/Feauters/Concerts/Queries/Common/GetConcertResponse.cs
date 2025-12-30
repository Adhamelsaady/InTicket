namespace InTicket.Application.Feauters.Concerts.Queries.Common;

public class GetConcertResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }

    public string Location { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public int Price { get; set; }
    public int AvailableTickets { get; set; }
    public string Artist { get; set; }
    public string? Organizer { get; set; }

    public int? DurationMinutes { get; set; }
    public int? MinimumAge { get; set; }
}