using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedMatchesEventsandTeamsEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FavoriteTeamId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BaseEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    TotalCapacity = table.Column<int>(type: "int", nullable: false),
                    AvailableTickets = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Concerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    TotalCapacity = table.Column<int>(type: "int", nullable: false),
                    AvailableTickets = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Artist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organizer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true),
                    MinimumAge = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stadium = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoundedYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    TotalCapacity = table.Column<int>(type: "int", nullable: false),
                    AvailableTickets = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HomeTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AwayTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    League = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Season = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Round = table.Column<int>(type: "int", nullable: true),
                    StadiumName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StadiumCapacity = table.Column<int>(type: "int", nullable: false),
                    FanPriorityBookingStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeneralBookingStart = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Concerts",
                columns: new[] { "Id", "Artist", "AvailableTickets", "CreatedAt", "Description", "DurationMinutes", "EventDate", "Location", "MinimumAge", "Organizer", "Price", "Title", "TotalCapacity", "UpdatedAt", "Venue" },
                values: new object[,]
                {
                    { new Guid("c1000000-0000-0000-0000-000000000001"), "Coldplay", 90000, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(9935), "Coldplay live in an unforgettable night of music", 130, new DateTime(2025, 3, 15, 19, 30, 0, 0, DateTimeKind.Utc), "", 12, "Live Nation", 150, "Coldplay: Music of the Spheres Tour", 90000, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(9935), "Wembley Stadium" },
                    { new Guid("c1000000-0000-0000-0000-000000000002"), "Adele", 20000, new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(564), "A soulful evening with Adele", 120, new DateTime(2025, 4, 10, 20, 0, 0, 0, DateTimeKind.Utc), "", 10, "AEG Presents", 180, "Adele Live in London", 20000, new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(565), "The O2 Arena" },
                    { new Guid("c1000000-0000-0000-0000-000000000003"), "Ed Sheeran", 60000, new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(591), "Ed Sheeran performing his greatest hits", 135, new DateTime(2025, 5, 5, 19, 0, 0, 0, DateTimeKind.Utc), "", 8, "Kilimanjaro Live", 140, "Ed Sheeran Mathematics Tour", 60000, new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(591), "Etihad Stadium" },
                    { new Guid("c1000000-0000-0000-0000-000000000004"), "Imagine Dragons", 62000, new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(594), "High-energy performance by Imagine Dragons", 125, new DateTime(2025, 6, 20, 20, 0, 0, 0, DateTimeKind.Utc), "", 10, "SJM Concerts", 130, "Imagine Dragons World Tour", 62000, new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(595), "Tottenham Hotspur Stadium" },
                    { new Guid("c1000000-0000-0000-0000-000000000005"), "Hans Zimmer", 5200, new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(597), "An orchestral experience of movie soundtracks", 150, new DateTime(2025, 7, 12, 19, 30, 0, 0, DateTimeKind.Utc), "", 12, "Semmel Concerts", 200, "Hans Zimmer Live", 5200, new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(598), "Royal Albert Hall" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "City", "FoundedYear", "Name", "Stadium" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Manchester", 1880, "Manchester City", "Etihad Stadium" },
                    { new Guid("12121212-1212-1212-1212-121212121212"), "Nottingham", 1865, "Nottingham Forest", "City Ground" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "London", 1886, "Arsenal", "Emirates Stadium" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Liverpool", 1892, "Liverpool", "Anfield" },
                    { new Guid("34343434-3434-3434-3434-343434343434"), "Bournemouth", 1899, "Bournemouth", "Vitality Stadium" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Manchester", 1878, "Manchester United", "Old Trafford" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "London", 1905, "Chelsea", "Stamford Bridge" },
                    { new Guid("56565656-5656-5656-5656-565656565656"), "Sheffield", 1889, "Sheffield United", "Bramall Lane" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "London", 1882, "Tottenham Hotspur", "Tottenham Hotspur Stadium" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Newcastle upon Tyne", 1892, "Newcastle United", "St James' Park" },
                    { new Guid("78787878-7878-7878-7878-787878787878"), "Burnley", 1882, "Burnley", "Turf Moor" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Birmingham", 1874, "Aston Villa", "Villa Park" },
                    { new Guid("90909090-9090-9090-9090-909090909090"), "Luton", 1885, "Luton Town", "Kenilworth Road" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "Brighton", 1901, "Brighton & Hove Albion", "Amex Stadium" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "London", 1895, "West Ham United", "London Stadium" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Wolverhampton", 1877, "Wolverhampton Wanderers", "Molineux Stadium" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Liverpool", 1878, "Everton", "Goodison Park" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "London", 1889, "Brentford", "Gtech Community Stadium" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "London", 1879, "Fulham", "Craven Cottage" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "London", 1905, "Crystal Palace", "Selhurst Park" }
                });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "AvailableTickets", "AwayTeamId", "CreatedAt", "Description", "EventDate", "FanPriorityBookingStart", "GeneralBookingStart", "HomeTeamId", "League", "Location", "Price", "Round", "Season", "StadiumCapacity", "StadiumName", "Title", "TotalCapacity", "UpdatedAt", "Venue" },
                values: new object[,]
                {
                    { new Guid("a1000000-0000-0000-0000-000000000001"), 55000, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(1674), "Premier League clash between title contenders", new DateTime(2025, 12, 28, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 5, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111"), "Premier League", "", 120, 18, "2025/2026", 0, "Etihad Stadium", "Manchester City vs Arsenal", 55000, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(1680), "Etihad Stadium" },
                    { new Guid("a1000000-0000-0000-0000-000000000002"), 54000, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6182), "Historic rivalry at Anfield", new DateTime(2025, 12, 29, 16, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 3, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 7, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333"), "Premier League", "", 130, 19, "2025/2026", 0, "Anfield", "Liverpool vs Manchester United", 54000, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6186), "Anfield" },
                    { new Guid("a1000000-0000-0000-0000-000000000003"), 41000, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6196), "London derby", new DateTime(2025, 12, 30, 17, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 5, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("55555555-5555-5555-5555-555555555555"), "Premier League", "", 110, 19, "2025/2026", 0, "Stamford Bridge", "Chelsea vs Tottenham Hotspur", 41000, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6196), "Stamford Bridge" },
                    { new Guid("a1000000-0000-0000-0000-000000000004"), 52000, new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6212), "Top-six battle", new DateTime(2025, 12, 31, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 7, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 11, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("77777777-7777-7777-7777-777777777777"), "Premier League", "", 95, 19, "2025/2026", 0, "St James' Park", "Newcastle United vs Aston Villa", 52000, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6212), "St James' Park" },
                    { new Guid("a1000000-0000-0000-0000-000000000005"), 60000, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6218), "North London derby", new DateTime(2026, 1, 1, 16, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 14, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), "Premier League", "", 140, 20, "2025/2026", 0, "Emirates Stadium", "Arsenal vs Tottenham Hotspur", 60000, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6218), "Emirates Stadium" },
                    { new Guid("a1000000-0000-0000-0000-000000000006"), 62500, new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6232), "London rivalry", new DateTime(2026, 1, 2, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 12, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 16, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Premier League", "", 90, 20, "2025/2026", 0, "London Stadium", "West Ham United vs Chelsea", 62500, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6232), "London Stadium" },
                    { new Guid("a1000000-0000-0000-0000-000000000007"), 39500, new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6238), "Merseyside derby", new DateTime(2026, 1, 3, 14, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Premier League", "", 100, 20, "2025/2026", 0, "Goodison Park", "Everton vs Liverpool", 39500, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6238), "Goodison Park" },
                    { new Guid("a1000000-0000-0000-0000-000000000008"), 74000, new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6242), "Old Trafford showdown", new DateTime(2026, 1, 4, 18, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 15, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 19, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444444"), "Premier League", "", 125, 21, "2025/2026", 0, "Old Trafford", "Manchester United vs Newcastle United", 74000, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6242), "Old Trafford" },
                    { new Guid("a1000000-0000-0000-0000-000000000009"), 32000, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6246), "South coast encounter", new DateTime(2026, 1, 5, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Premier League", "", 70, 21, "2025/2026", 0, "Amex Stadium", "Brighton vs Brentford", 32000, new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6246), "Amex Stadium" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FavoriteTeamId",
                table: "AspNetUsers",
                column: "FavoriteTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamId",
                table: "Matches",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teams_FavoriteTeamId",
                table: "AspNetUsers",
                column: "FavoriteTeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teams_FavoriteTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BaseEvents");

            migrationBuilder.DropTable(
                name: "Concerts");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FavoriteTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteTeamId",
                table: "AspNetUsers");
        }
    }
}
