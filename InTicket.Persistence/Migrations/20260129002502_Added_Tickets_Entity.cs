using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Tickets_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Concerts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BaseEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EventTickets",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HolderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TicketClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BookedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeldAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeldExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "MatchTickets",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HolderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TicketClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BookedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeldAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeldExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeTeamTicket = table.Column<bool>(type: "bit", nullable: false),
                    TicketClass = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_MatchTickets_AspNetUsers_HolderId",
                        column: x => x.HolderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MatchTickets_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HolderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TicketClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BookedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeldAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeldExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Ticket_AspNetUsers_HolderId",
                        column: x => x.HolderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(7727), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(7728) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8240), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8240) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8252), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8252) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8256), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8256) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8259), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8259) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 993, DateTimeKind.Utc).AddTicks(2730), false, new DateTime(2026, 1, 29, 0, 24, 59, 993, DateTimeKind.Utc).AddTicks(2732) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(18), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(19) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(31), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(32) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(55), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(55) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(59), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(60) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000006"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(66), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(67) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000007"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(70), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(71) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000008"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(74), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(74) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000009"),
                columns: new[] { "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(77), false, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(78) });

            migrationBuilder.CreateIndex(
                name: "IX_EventTickets_ConcertId",
                table: "EventTickets",
                column: "ConcertId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTickets_HolderId",
                table: "EventTickets",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTickets_HolderId",
                table: "MatchTickets",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTickets_MatchId",
                table: "MatchTickets",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_HolderId",
                table: "Ticket",
                column: "HolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTickets");

            migrationBuilder.DropTable(
                name: "MatchTickets");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BaseEvents");

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 194, DateTimeKind.Utc).AddTicks(7905), new DateTime(2026, 1, 19, 16, 4, 50, 194, DateTimeKind.Utc).AddTicks(7911) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 194, DateTimeKind.Utc).AddTicks(8714), new DateTime(2026, 1, 19, 16, 4, 50, 194, DateTimeKind.Utc).AddTicks(8714) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 194, DateTimeKind.Utc).AddTicks(8720), new DateTime(2026, 1, 19, 16, 4, 50, 194, DateTimeKind.Utc).AddTicks(8721) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 194, DateTimeKind.Utc).AddTicks(8724), new DateTime(2026, 1, 19, 16, 4, 50, 194, DateTimeKind.Utc).AddTicks(8724) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 194, DateTimeKind.Utc).AddTicks(8729), new DateTime(2026, 1, 19, 16, 4, 50, 194, DateTimeKind.Utc).AddTicks(8729) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(3202), new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(3209) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7836), new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7837) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7844), new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7844) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7849), new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7850) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7872), new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7872) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000006"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7890), new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7890) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000007"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7894), new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7894) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000008"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7898), new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7898) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000009"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7902), new DateTime(2026, 1, 19, 16, 4, 50, 193, DateTimeKind.Utc).AddTicks(7902) });
        }
    }
}
