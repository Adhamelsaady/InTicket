using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Delegations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Delegations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DelegatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DelegateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delegations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delegations_AspNetUsers_DelegateId",
                        column: x => x.DelegateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Delegations_AspNetUsers_DelegatorId",
                        column: x => x.DelegatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 528, DateTimeKind.Utc).AddTicks(7205), new DateTime(2026, 1, 18, 13, 26, 52, 528, DateTimeKind.Utc).AddTicks(7207) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 528, DateTimeKind.Utc).AddTicks(7731), new DateTime(2026, 1, 18, 13, 26, 52, 528, DateTimeKind.Utc).AddTicks(7731) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 528, DateTimeKind.Utc).AddTicks(7747), new DateTime(2026, 1, 18, 13, 26, 52, 528, DateTimeKind.Utc).AddTicks(7748) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 528, DateTimeKind.Utc).AddTicks(7751), new DateTime(2026, 1, 18, 13, 26, 52, 528, DateTimeKind.Utc).AddTicks(7752) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 528, DateTimeKind.Utc).AddTicks(7758), new DateTime(2026, 1, 18, 13, 26, 52, 528, DateTimeKind.Utc).AddTicks(7758) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(3194), new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(3200) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8480), new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8482) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8493), new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8493) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8500), new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8500) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8517), new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8517) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000006"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8528), new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8528) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000007"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8533), new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8533) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000008"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8537), new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8537) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000009"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8541), new DateTime(2026, 1, 18, 13, 26, 52, 527, DateTimeKind.Utc).AddTicks(8541) });

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_DelegateId",
                table: "Delegations",
                column: "DelegateId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_DelegatorId",
                table: "Delegations",
                column: "DelegatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Delegations");

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(9935), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(9935) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(564), new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(565) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(591), new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(591) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(594), new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(595) });

            migrationBuilder.UpdateData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("c1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(597), new DateTime(2025, 12, 28, 12, 25, 46, 160, DateTimeKind.Utc).AddTicks(598) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(1674), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(1680) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6182), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6186) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6196), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6196) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6212), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6212) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6218), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6218) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000006"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6232), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6232) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000007"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6238), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6238) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000008"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6242), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6242) });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1000000-0000-0000-0000-000000000009"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6246), new DateTime(2025, 12, 28, 12, 25, 46, 159, DateTimeKind.Utc).AddTicks(6246) });
        }
    }
}
