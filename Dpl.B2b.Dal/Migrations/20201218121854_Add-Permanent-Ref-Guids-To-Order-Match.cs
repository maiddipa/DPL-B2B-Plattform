using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddPermanentRefGuidsToOrderMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RefLmsPermanentAvailabilityRowGuid",
                table: "OrderMatches",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RefLmsPermanentDeliveryRowGuid",
                table: "OrderMatches",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 2,
                column: "Iso2Code",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefLmsPermanentAvailabilityRowGuid",
                table: "OrderMatches");

            migrationBuilder.DropColumn(
                name: "RefLmsPermanentDeliveryRowGuid",
                table: "OrderMatches");

            migrationBuilder.UpdateData(
                table: "CountryStates",
                keyColumn: "Id",
                keyValue: 2,
                column: "Iso2Code",
                value: "BY");
        }
    }
}
