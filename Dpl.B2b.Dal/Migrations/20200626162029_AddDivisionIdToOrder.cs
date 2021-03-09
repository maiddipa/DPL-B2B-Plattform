using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddDivisionIdToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DivisionId",
                table: "Orders",
                nullable: true);

            migrationBuilder.Sql(@"
UPDATE O SET O.DivisionId = LL.CustomerDivisionId
FROM Orders O
INNER JOIN LoadingLocations LL ON LL.Id = O.LoadingLocationId
WHERE O.DivisionId IS NULL
");

            migrationBuilder.AlterColumn<int>(
                name: "DivisionId",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DivisionId",
                table: "Orders",
                column: "DivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomerDivisions_DivisionId",
                table: "Orders",
                column: "DivisionId",
                principalTable: "CustomerDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomerDivisions_DivisionId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DivisionId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "Orders");
        }
    }
}
