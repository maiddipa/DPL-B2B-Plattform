using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class IndexesForDplEmployeeCustomerSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefLtmsAccountIdString",
                table: "PostingAccounts",
                maxLength: 255,
                nullable: false,
                computedColumnSql: "CONVERT(nvarchar(255), [RefLtmsAccountId])");

            migrationBuilder.AddColumn<string>(
                name: "CustomerNumberString",
                table: "Customers",
                maxLength: 255,
                nullable: false,
                computedColumnSql: "CONVERT(nvarchar(255), [CustomerNumber])");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_DisplayName",
                table: "PostingAccounts",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_RefLtmsAccountIdString",
                table: "PostingAccounts",
                column: "RefLtmsAccountIdString");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerNumber",
                table: "Customers",
                column: "CustomerNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerNumberString",
                table: "Customers",
                column: "CustomerNumberString");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name",
                table: "Customers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_Name",
                table: "CustomerDivisions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDivisions_ShortName",
                table: "CustomerDivisions",
                column: "ShortName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostingAccounts_DisplayName",
                table: "PostingAccounts");

            migrationBuilder.DropIndex(
                name: "IX_PostingAccounts_RefLtmsAccountIdString",
                table: "PostingAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CustomerNumber",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CustomerNumberString",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Name",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDivisions_Name",
                table: "CustomerDivisions");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDivisions_ShortName",
                table: "CustomerDivisions");

            migrationBuilder.DropColumn(
                name: "RefLtmsAccountIdString",
                table: "PostingAccounts");

            migrationBuilder.DropColumn(
                name: "CustomerNumberString",
                table: "Customers");
        }
    }
}
