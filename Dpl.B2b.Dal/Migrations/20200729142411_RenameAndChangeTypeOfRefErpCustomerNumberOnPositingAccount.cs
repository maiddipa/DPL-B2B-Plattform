using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class RenameAndChangeTypeOfRefErpCustomerNumberOnPositingAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostingAccounts_RefLtmsAccountCustomerNumber",
                table: "PostingAccounts");

            migrationBuilder.DropColumn(
                name: "RefLtmsAccountCustomerNumber",
                table: "PostingAccounts");

            migrationBuilder.AddColumn<int>(
                name: "RefErpCustomerNumber",
                table: "PostingAccounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefErpCustomerNumber",
                table: "PostingAccounts");

            migrationBuilder.AddColumn<string>(
                name: "RefLtmsAccountCustomerNumber",
                table: "PostingAccounts",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_RefLtmsAccountCustomerNumber",
                table: "PostingAccounts",
                column: "RefLtmsAccountCustomerNumber");
        }
    }
}
