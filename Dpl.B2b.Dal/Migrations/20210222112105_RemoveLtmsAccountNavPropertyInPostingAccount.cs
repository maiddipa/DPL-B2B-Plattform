using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class RemoveLtmsAccountNavPropertyInPostingAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostingAccounts_Partners_PartnerId",
                table: "PostingAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostingAccounts_Accounts_RefLtmsAccountId",
                table: "PostingAccounts");

            migrationBuilder.DropIndex(
                name: "IX_PostingAccounts_PartnerId",
                table: "PostingAccounts");

            migrationBuilder.AddColumn<int>(
                name: "PartnerId1",
                table: "PostingAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_PartnerId1",
                table: "PostingAccounts",
                column: "PartnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PostingAccounts_Partners_PartnerId1",
                table: "PostingAccounts",
                column: "PartnerId1",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostingAccounts_Partners_PartnerId1",
                table: "PostingAccounts");

            migrationBuilder.DropIndex(
                name: "IX_PostingAccounts_PartnerId1",
                table: "PostingAccounts");

            migrationBuilder.DropColumn(
                name: "PartnerId1",
                table: "PostingAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_PostingAccounts_PartnerId",
                table: "PostingAccounts",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostingAccounts_Partners_PartnerId",
                table: "PostingAccounts",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostingAccounts_Accounts_RefLtmsAccountId",
                table: "PostingAccounts",
                column: "RefLtmsAccountId",
                principalSchema: "LTMS",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
