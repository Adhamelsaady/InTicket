using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixDelegationCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 189, DateTimeKind.Utc).AddTicks(5006), new DateTime(2026, 1, 19, 13, 21, 37, 189, DateTimeKind.Utc).AddTicks(5010) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 189, DateTimeKind.Utc).AddTicks(5725), new DateTime(2026, 1, 19, 13, 21, 37, 189, DateTimeKind.Utc).AddTicks(5726) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 189, DateTimeKind.Utc).AddTicks(5741), new DateTime(2026, 1, 19, 13, 21, 37, 189, DateTimeKind.Utc).AddTicks(5741) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 189, DateTimeKind.Utc).AddTicks(5745), new DateTime(2026, 1, 19, 13, 21, 37, 189, DateTimeKind.Utc).AddTicks(5745) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 189, DateTimeKind.Utc).AddTicks(5750), new DateTime(2026, 1, 19, 13, 21, 37, 189, DateTimeKind.Utc).AddTicks(5750) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 187, DateTimeKind.Utc).AddTicks(6897), new DateTime(2026, 1, 19, 13, 21, 37, 187, DateTimeKind.Utc).AddTicks(6903) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3384), new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3388) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3402), new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3402) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3408), new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3409) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3415), new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3416) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000006"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3432), new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3432) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000007"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3438), new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3439) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000008"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3466), new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3466) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000009"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3472), new DateTime(2026, 1, 19, 13, 21, 37, 188, DateTimeKind.Utc).AddTicks(3472) });
        }
    }
}
