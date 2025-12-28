using InTicket.Domain;

public static class MatchSeed
{
    public static readonly List<Match> PremierLeagueMatches = new()
    {
        new Match
        {
            Id = Guid.Parse("a1000000-0000-0000-0000-000000000001"),
            Title = "Manchester City vs Arsenal",
            Description = "Premier League clash between title contenders",
            EventDate = new DateTime(2025, 12, 28, 18, 30, 0, DateTimeKind.Utc),
            Venue = "Etihad Stadium",
            StadiumName = "Etihad Stadium",
            Price = 120,
            TotalCapacity = 55000,
            AvailableTickets = 55000,

            HomeTeamId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            AwayTeamId = Guid.Parse("22222222-2222-2222-2222-222222222222"),

            League = "Premier League",
            Season = "2025/2026",
            Round = 18,

            FanPriorityBookingStart = new DateTime(2025, 12, 1, 10, 0, 0, DateTimeKind.Utc),
            GeneralBookingStart     = new DateTime(2025, 12, 5, 10, 0, 0, DateTimeKind.Utc)
        },

        new Match
        {
            Id = Guid.Parse("a1000000-0000-0000-0000-000000000002"),
            Title = "Liverpool vs Manchester United",
            Description = "Historic rivalry at Anfield",
            EventDate = new DateTime(2025, 12, 29, 16, 30, 0, DateTimeKind.Utc),
            Venue = "Anfield",
            StadiumName = "Anfield",
            Price = 130,
            TotalCapacity = 54000,
            AvailableTickets = 54000,

            HomeTeamId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            AwayTeamId = Guid.Parse("44444444-4444-4444-4444-444444444444"),

            League = "Premier League",
            Season = "2025/2026",
            Round = 19,

            FanPriorityBookingStart = new DateTime(2025, 12, 3, 10, 0, 0, DateTimeKind.Utc),
            GeneralBookingStart     = new DateTime(2025, 12, 7, 10, 0, 0, DateTimeKind.Utc)
        },

        new Match
        {
            Id = Guid.Parse("a1000000-0000-0000-0000-000000000003"),
            Title = "Chelsea vs Tottenham Hotspur",
            Description = "London derby",
            EventDate = new DateTime(2025, 12, 30, 17, 30, 0, DateTimeKind.Utc),
            Venue = "Stamford Bridge",
            StadiumName = "Stamford Bridge",
            Price = 110,
            TotalCapacity = 41000,
            AvailableTickets = 41000,

            HomeTeamId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            AwayTeamId = Guid.Parse("66666666-6666-6666-6666-666666666666"),

            League = "Premier League",
            Season = "2025/2026",
            Round = 19,

            FanPriorityBookingStart = new DateTime(2025, 12, 5, 10, 0, 0, DateTimeKind.Utc),
            GeneralBookingStart     = new DateTime(2025, 12, 9, 10, 0, 0, DateTimeKind.Utc)
        },

        new Match
        {
            Id = Guid.Parse("a1000000-0000-0000-0000-000000000004"),
            Title = "Newcastle United vs Aston Villa",
            Description = "Top-six battle",
            EventDate = new DateTime(2025, 12, 31, 15, 0, 0, DateTimeKind.Utc),
            Venue = "St James' Park",
            StadiumName = "St James' Park",
            Price = 95,
            TotalCapacity = 52000,
            AvailableTickets = 52000,

            HomeTeamId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
            AwayTeamId = Guid.Parse("88888888-8888-8888-8888-888888888888"),

            League = "Premier League",
            Season = "2025/2026",
            Round = 19,

            FanPriorityBookingStart = new DateTime(2025, 12, 7, 10, 0, 0, DateTimeKind.Utc),
            GeneralBookingStart     = new DateTime(2025, 12, 11, 10, 0, 0, DateTimeKind.Utc)
        },

        new Match
        {
            Id = Guid.Parse("a1000000-0000-0000-0000-000000000005"),
            Title = "Arsenal vs Tottenham Hotspur",
            Description = "North London derby",
            EventDate = new DateTime(2026, 1, 1, 16, 30, 0, DateTimeKind.Utc),
            Venue = "Emirates Stadium",
            StadiumName = "Emirates Stadium",
            Price = 140,
            TotalCapacity = 60000,
            AvailableTickets = 60000,

            HomeTeamId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            AwayTeamId = Guid.Parse("66666666-6666-6666-6666-666666666666"),

            League = "Premier League",
            Season = "2025/2026",
            Round = 20,

            FanPriorityBookingStart = new DateTime(2025, 12, 10, 10, 0, 0, DateTimeKind.Utc),
            GeneralBookingStart     = new DateTime(2025, 12, 14, 10, 0, 0, DateTimeKind.Utc)
        },

        new Match
        {
            Id = Guid.Parse("a1000000-0000-0000-0000-000000000006"),
            Title = "West Ham United vs Chelsea",
            Description = "London rivalry",
            EventDate = new DateTime(2026, 1, 2, 18, 30, 0, DateTimeKind.Utc),
            Venue = "London Stadium",
            StadiumName = "London Stadium",
            Price = 90,
            TotalCapacity = 62500,
            AvailableTickets = 62500,

            HomeTeamId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            AwayTeamId = Guid.Parse("55555555-5555-5555-5555-555555555555"),

            League = "Premier League",
            Season = "2025/2026",
            Round = 20,

            FanPriorityBookingStart = new DateTime(2025, 12, 12, 10, 0, 0, DateTimeKind.Utc),
            GeneralBookingStart     = new DateTime(2025, 12, 16, 10, 0, 0, DateTimeKind.Utc)
        },

        new Match
        {
            Id = Guid.Parse("a1000000-0000-0000-0000-000000000007"),
            Title = "Everton vs Liverpool",
            Description = "Merseyside derby",
            EventDate = new DateTime(2026, 1, 3, 14, 0, 0, DateTimeKind.Utc),
            Venue = "Goodison Park",
            StadiumName = "Goodison Park",
            Price = 100,
            TotalCapacity = 39500,
            AvailableTickets = 39500,

            HomeTeamId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            AwayTeamId = Guid.Parse("33333333-3333-3333-3333-333333333333"),

            League = "Premier League",
            Season = "2025/2026",
            Round = 20
        },

        new Match
        {
            Id = Guid.Parse("a1000000-0000-0000-0000-000000000008"),
            Title = "Manchester United vs Newcastle United",
            Description = "Old Trafford showdown",
            EventDate = new DateTime(2026, 1, 4, 18, 30, 0, DateTimeKind.Utc),
            Venue = "Old Trafford",
            StadiumName = "Old Trafford",
            Price = 125,
            TotalCapacity = 74000,
            AvailableTickets = 74000,

            HomeTeamId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            AwayTeamId = Guid.Parse("77777777-7777-7777-7777-777777777777"),

            League = "Premier League",
            Season = "2025/2026",
            Round = 21,

            FanPriorityBookingStart = new DateTime(2025, 12, 15, 10, 0, 0, DateTimeKind.Utc),
            GeneralBookingStart     = new DateTime(2025, 12, 19, 10, 0, 0, DateTimeKind.Utc)
        },

        new Match
        {
            Id = Guid.Parse("a1000000-0000-0000-0000-000000000009"),
            Title = "Brighton vs Brentford",
            Description = "South coast encounter",
            EventDate = new DateTime(2026, 1, 5, 15, 0, 0, DateTimeKind.Utc),
            Venue = "Amex Stadium",
            StadiumName = "Amex Stadium",
            Price = 70,
            TotalCapacity = 32000,
            AvailableTickets = 32000,

            HomeTeamId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            AwayTeamId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),

            League = "Premier League",
            Season = "2025/2026",
            Round = 21,

            FanPriorityBookingStart = new DateTime(2025, 12, 18, 10, 0, 0, DateTimeKind.Utc),
            GeneralBookingStart     = new DateTime(2025, 12, 31, 10, 0, 0, DateTimeKind.Utc)
        }
    };
}
