using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fixe_Tickets_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketClassId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketClassId",
                table: "MatchTickets");

            migrationBuilder.DropColumn(
                name: "AvailableTickets",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TotalCapacity",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TicketClassId",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "AvailableTickets",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "TotalCapacity",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "AvailableTickets",
                table: "BaseEvents");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BaseEvents");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BaseEvents");

            migrationBuilder.DropColumn(
                name: "TotalCapacity",
                table: "BaseEvents");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BaseEvents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TicketClassId",
                table: "Ticket",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TicketClassId",
                table: "MatchTickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "AvailableTickets",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCapacity",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "TicketClassId",
                table: "EventTickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "AvailableTickets",
                table: "Concerts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Concerts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Concerts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCapacity",
                table: "Concerts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Concerts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AvailableTickets",
                table: "BaseEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BaseEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "BaseEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCapacity",
                table: "BaseEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "BaseEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000001"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 90000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(7727), 150, 90000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(7728) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000002"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 20000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8240), 180, 20000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8240) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000003"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 60000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8252), 140, 60000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8252) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000004"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 62000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8256), 130, 62000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8256) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000005"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 5200, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8259), 200, 5200, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(8259) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000001"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 55000, new DateTime(2026, 1, 29, 0, 24, 59, 993, DateTimeKind.Utc).AddTicks(2730), 120, 55000, new DateTime(2026, 1, 29, 0, 24, 59, 993, DateTimeKind.Utc).AddTicks(2732) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000002"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 54000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(18), 130, 54000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(19) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000003"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 41000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(31), 110, 41000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(32) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000004"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 52000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(55), 95, 52000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(55) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000005"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 60000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(59), 140, 60000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(60) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000006"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 62500, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(66), 90, 62500, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(67) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000007"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 39500, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(70), 100, 39500, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(71) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000008"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 74000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(74), 125, 74000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(74) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000009"),
                columns: new[] { "AvailableTickets", "CreatedAt", "Price", "TotalCapacity", "UpdatedAt" },
                values: new object[] { 32000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(77), 70, 32000, new DateTime(2026, 1, 29, 0, 24, 59, 994, DateTimeKind.Utc).AddTicks(78) });
        }
    }
}
