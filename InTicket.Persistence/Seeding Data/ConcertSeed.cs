using InTicket.Domain;

public static class ConcertSeed
{
    public static readonly List<Concert> Concerts = new()
    {
        new Concert
        {
            Id = Guid.Parse("c1000000-0000-0000-0000-000000000001"),
            Title = "Coldplay: Music of the Spheres Tour",
            Description = "Coldplay live in an unforgettable night of music",
            EventDate = new DateTime(2025, 3, 15, 19, 30, 0, DateTimeKind.Utc),
            Venue = "Wembley Stadium",
            Price = 150,
            TotalCapacity = 90000,
            AvailableTickets = 90000,

            Artist = "Coldplay",
            Organizer = "Live Nation",
            DurationMinutes = 130,
            MinimumAge = 12,
        },

        new Concert
        {
            Id = Guid.Parse("c1000000-0000-0000-0000-000000000002"),
            Title = "Adele Live in London",
            Description = "A soulful evening with Adele",
            EventDate = new DateTime(2025, 4, 10, 20, 0, 0, DateTimeKind.Utc),
            Venue = "The O2 Arena",
            Price = 180,
            TotalCapacity = 20000,
            AvailableTickets = 20000,

            Artist = "Adele",
            Organizer = "AEG Presents",
            DurationMinutes = 120,
            MinimumAge = 10,
           
        },

        new Concert
        {
            Id = Guid.Parse("c1000000-0000-0000-0000-000000000003"),
            Title = "Ed Sheeran Mathematics Tour",
            Description = "Ed Sheeran performing his greatest hits",
            EventDate = new DateTime(2025, 5, 5, 19, 0, 0, DateTimeKind.Utc),
            Venue = "Etihad Stadium",
            Price = 140,
            TotalCapacity = 60000,
            AvailableTickets = 60000,

            Artist = "Ed Sheeran",
            Organizer = "Kilimanjaro Live",
            DurationMinutes = 135,
            MinimumAge = 8,
     
        },

        new Concert
        {
            Id = Guid.Parse("c1000000-0000-0000-0000-000000000004"),
            Title = "Imagine Dragons World Tour",
            Description = "High-energy performance by Imagine Dragons",
            EventDate = new DateTime(2025, 6, 20, 20, 0, 0, DateTimeKind.Utc),
            Venue = "Tottenham Hotspur Stadium",
            Price = 130,
            TotalCapacity = 62000,
            AvailableTickets = 62000,

            Artist = "Imagine Dragons",
            Organizer = "SJM Concerts",
            DurationMinutes = 125,
            MinimumAge = 10,
  
        },

        new Concert
        {
            Id = Guid.Parse("c1000000-0000-0000-0000-000000000005"),
            Title = "Hans Zimmer Live",
            Description = "An orchestral experience of movie soundtracks",
            EventDate = new DateTime(2025, 7, 12, 19, 30, 0, DateTimeKind.Utc),
            Venue = "Royal Albert Hall",
            Price = 200,
            TotalCapacity = 5200,
            AvailableTickets = 5200,

            Artist = "Hans Zimmer",
            Organizer = "Semmel Concerts",
            DurationMinutes = 150,
            MinimumAge = 12,
     
        }
    };
}
