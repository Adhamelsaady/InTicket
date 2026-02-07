using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class remove_concert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTickets");

            migrationBuilder.DropTable(
                name: "Concerts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Concerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Artist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true),
                    MinimumAge = table.Column<int>(type: "int", nullable: true),
                    Organizer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventTickets",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HolderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeldAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeldExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ConcertId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketClass = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_EventTickets_AspNetUsers_HolderId",
                        column: x => x.HolderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EventTickets_Concerts_ConcertId",
                        column: x => x.ConcertId,
                        principalTable: "Concerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Concerts",
                columns: new[] { "Id", "Artist", "BookingStartDate", "Description", "DurationMinutes", "EventDate", "IsActive", "Location", "MinimumAge", "Organizer", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("c1000000-0000-0000-0000-000000000001"), "Coldplay", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Coldplay live in an unforgettable night of music", 130, new DateTime(2025, 3, 15, 19, 30, 0, 0, DateTimeKind.Utc), false, "", 12, "Live Nation", "Coldplay: Music of the Spheres Tour", "Wembley Stadium" },
                    { new Guid("c1000000-0000-0000-0000-000000000002"), "Adele", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A soulful evening with Adele", 120, new DateTime(2025, 4, 10, 20, 0, 0, 0, DateTimeKind.Utc), false, "", 10, "AEG Presents", "Adele Live in London", "The O2 Arena" },
                    { new Guid("c1000000-0000-0000-0000-000000000003"), "Ed Sheeran", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ed Sheeran performing his greatest hits", 135, new DateTime(2025, 5, 5, 19, 0, 0, 0, DateTimeKind.Utc), false, "", 8, "Kilimanjaro Live", "Ed Sheeran Mathematics Tour", "Etihad Stadium" },
                    { new Guid("c1000000-0000-0000-0000-000000000004"), "Imagine Dragons", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "High-energy performance by Imagine Dragons", 125, new DateTime(2025, 6, 20, 20, 0, 0, 0, DateTimeKind.Utc), false, "", 10, "SJM Concerts", "Imagine Dragons World Tour", "Tottenham Hotspur Stadium" },
                    { new Guid("c1000000-0000-0000-0000-000000000005"), "Hans Zimmer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "An orchestral experience of movie soundtracks", 150, new DateTime(2025, 7, 12, 19, 30, 0, 0, DateTimeKind.Utc), false, "", 12, "Semmel Concerts", "Hans Zimmer Live", "Royal Albert Hall" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventTickets_ConcertId",
                table: "EventTickets",
                column: "ConcertId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTickets_HolderId",
                table: "EventTickets",
                column: "HolderId");
        }
    }
}
