using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddEmployeeNoteToOrderLoad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderLoadId",
                table: "EmployeeNote",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeNote_OrderLoadId",
                table: "EmployeeNote",
                column: "OrderLoadId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeNote_OrderLoad_OrderLoadId",
                table: "EmployeeNote",
                column: "OrderLoadId",
                principalTable: "OrderLoad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeNote_OrderLoad_OrderLoadId",
                table: "EmployeeNote");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeNote_OrderLoadId",
                table: "EmployeeNote");

            migrationBuilder.DropColumn(
                name: "OrderLoadId",
                table: "EmployeeNote");
        }
    }
}
