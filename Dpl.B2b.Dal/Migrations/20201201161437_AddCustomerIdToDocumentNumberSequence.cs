using Dpl.B2b.Dal.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddCustomerIdToDocumentNumberSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SetCustomerIdToDocumentNumberSequence();
            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "DocumentNumberSequences",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "DocumentNumberSequences",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
