using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_dbset_for_tickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AspNetUsers_HolderId",
                table: "Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.RenameTable(
                name: "Ticket",
                newName: "Tickets");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_HolderId",
                table: "Tickets",
                newName: "IX_Tickets_HolderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_HolderId",
                table: "Tickets",
                column: "HolderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_HolderId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "Ticket");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_HolderId",
                table: "Ticket",
                newName: "IX_Ticket_HolderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AspNetUsers_HolderId",
                table: "Ticket",
                column: "HolderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
