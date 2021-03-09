using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class fixpartnerdirectoryidfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerDirectoryAccesses_PartnerDirectories_DirectoryId1",
                table: "PartnerDirectoryAccesses");

            migrationBuilder.DropIndex(
                name: "IX_PartnerDirectoryAccesses_DirectoryId1",
                table: "PartnerDirectoryAccesses");

            migrationBuilder.DropColumn(
                name: "DirectoryId1",
                table: "PartnerDirectoryAccesses");

            migrationBuilder.AlterColumn<int>(
                name: "DirectoryId",
                table: "PartnerDirectoryAccesses",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartnerDirectoryAccesses_DirectoryId",
                table: "PartnerDirectoryAccesses",
                column: "DirectoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerDirectoryAccesses_PartnerDirectories_DirectoryId",
                table: "PartnerDirectoryAccesses",
                column: "DirectoryId",
                principalTable: "PartnerDirectories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerDirectoryAccesses_PartnerDirectories_DirectoryId",
                table: "PartnerDirectoryAccesses");

            migrationBuilder.DropIndex(
                name: "IX_PartnerDirectoryAccesses_DirectoryId",
                table: "PartnerDirectoryAccesses");

            migrationBuilder.AlterColumn<string>(
                name: "DirectoryId",
                table: "PartnerDirectoryAccesses",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "DirectoryId1",
                table: "PartnerDirectoryAccesses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartnerDirectoryAccesses_DirectoryId1",
                table: "PartnerDirectoryAccesses",
                column: "DirectoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerDirectoryAccesses_PartnerDirectories_DirectoryId1",
                table: "PartnerDirectoryAccesses",
                column: "DirectoryId1",
                principalTable: "PartnerDirectories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
