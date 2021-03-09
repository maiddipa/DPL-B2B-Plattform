using Dpl.B2b.Dal.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddCustomerToPostingAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "PostingAccounts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_CustomerId",
                table: "PostingAccounts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostingAccounts_Customers_CustomerId",
                table: "PostingAccounts",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.SetCustomerIdForPostingAccounts();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostingAccounts_Customers_CustomerId",
                table: "PostingAccounts");

            migrationBuilder.DropIndex(
                name: "IX_PostingAccounts_CustomerId",
                table: "PostingAccounts");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "PostingAccounts");
        }
    }
}
