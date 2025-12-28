using InTicket.Domain;

public static class TeamSeed
{
    public static readonly List<Team> PremierLeagueTeams = new()
    {
        new Team
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Manchester City",
            Stadium = "Etihad Stadium",
            City = "Manchester",
            FoundedYear = 1880
        },
        new Team
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Arsenal",
            Stadium = "Emirates Stadium",
            City = "London",
            FoundedYear = 1886
        },
        new Team
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Liverpool",
            Stadium = "Anfield",
            City = "Liverpool",
            FoundedYear = 1892
        },
        new Team
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Name = "Manchester United",
            Stadium = "Old Trafford",
            City = "Manchester",
            FoundedYear = 1878
        },
        new Team
        {
            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            Name = "Chelsea",
            Stadium = "Stamford Bridge",
            City = "London",
            FoundedYear = 1905
        },
        new Team
        {
            Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            Name = "Tottenham Hotspur",
            Stadium = "Tottenham Hotspur Stadium",
            City = "London",
            FoundedYear = 1882
        },
        new Team
        {
            Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
            Name = "Newcastle United",
            Stadium = "St James' Park",
            City = "Newcastle upon Tyne",
            FoundedYear = 1892
        },
        new Team
        {
            Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            Name = "Aston Villa",
            Stadium = "Villa Park",
            City = "Birmingham",
            FoundedYear = 1874
        },
        new Team
        {
            Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            Name = "Brighton & Hove Albion",
            Stadium = "Amex Stadium",
            City = "Brighton",
            FoundedYear = 1901
        },
        new Team
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Name = "West Ham United",
            Stadium = "London Stadium",
            City = "London",
            FoundedYear = 1895
        },
        new Team
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Name = "Wolverhampton Wanderers",
            Stadium = "Molineux Stadium",
            City = "Wolverhampton",
            FoundedYear = 1877
        },
        new Team
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            Name = "Everton",
            Stadium = "Goodison Park",
            City = "Liverpool",
            FoundedYear = 1878
        },
        new Team
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            Name = "Brentford",
            Stadium = "Gtech Community Stadium",
            City = "London",
            FoundedYear = 1889
        },
        new Team
        {
            Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            Name = "Fulham",
            Stadium = "Craven Cottage",
            City = "London",
            FoundedYear = 1879
        },
        new Team
        {
            Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
            Name = "Crystal Palace",
            Stadium = "Selhurst Park",
            City = "London",
            FoundedYear = 1905
        },
        new Team
        {
            Id = Guid.Parse("12121212-1212-1212-1212-121212121212"),
            Name = "Nottingham Forest",
            Stadium = "City Ground",
            City = "Nottingham",
            FoundedYear = 1865
        },
        new Team
        {
            Id = Guid.Parse("34343434-3434-3434-3434-343434343434"),
            Name = "Bournemouth",
            Stadium = "Vitality Stadium",
            City = "Bournemouth",
            FoundedYear = 1899
        },
        new Team
        {
            Id = Guid.Parse("56565656-5656-5656-5656-565656565656"),
            Name = "Sheffield United",
            Stadium = "Bramall Lane",
            City = "Sheffield",
            FoundedYear = 1889
        },
        new Team
        {
            Id = Guid.Parse("78787878-7878-7878-7878-787878787878"),
            Name = "Burnley",
            Stadium = "Turf Moor",
            City = "Burnley",
            FoundedYear = 1882
        },
        new Team
        {
            Id = Guid.Parse("90909090-9090-9090-9090-909090909090"),
            Name = "Luton Town",
            Stadium = "Kenilworth Road",
            City = "Luton",
            FoundedYear = 1885
        }
    };
}
