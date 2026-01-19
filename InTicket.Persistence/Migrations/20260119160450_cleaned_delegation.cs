using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class cleaned_delegation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "Delegations");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Delegations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                table: "Delegations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 735, DateTimeKind.Utc).AddTicks(5510), new DateTime(2026, 1, 19, 13, 23, 37, 735, DateTimeKind.Utc).AddTicks(5514) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 735, DateTimeKind.Utc).AddTicks(6059), new DateTime(2026, 1, 19, 13, 23, 37, 735, DateTimeKind.Utc).AddTicks(6059) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 735, DateTimeKind.Utc).AddTicks(6072), new DateTime(2026, 1, 19, 13, 23, 37, 735, DateTimeKind.Utc).AddTicks(6072) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 735, DateTimeKind.Utc).AddTicks(6075), new DateTime(2026, 1, 19, 13, 23, 37, 735, DateTimeKind.Utc).AddTicks(6075) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 735, DateTimeKind.Utc).AddTicks(6079), new DateTime(2026, 1, 19, 13, 23, 37, 735, DateTimeKind.Utc).AddTicks(6079) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(1820), new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(1826) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6243), new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6245) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6252), new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6252) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6257), new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6257) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6261), new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6262) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000006"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6292), new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6292) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000007"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6297), new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6297) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000008"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6301), new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6301) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000009"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6305), new DateTime(2026, 1, 19, 13, 23, 37, 734, DateTimeKind.Utc).AddTicks(6305) });
        }
    }
}
