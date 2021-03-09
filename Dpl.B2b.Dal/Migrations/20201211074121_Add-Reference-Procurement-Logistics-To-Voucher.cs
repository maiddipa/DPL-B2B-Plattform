using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddReferenceProcurementLogisticsToVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerReference",
                table: "Vouchers",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ProcurementLogistics",
                table: "Vouchers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerReference",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "ProcurementLogistics",
                table: "Vouchers");
        }
    }
}
