using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class FixMigrationAfterAddNewLoadCarrier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LoadCarriers",
                columns: new[] { "Id", "Name", "Order", "QualityId", "RefLmsQuality2PalletId", "RefLtmsPalletId", "TypeId" },
                values: new object[,]
                {
                    { 311, "HDB1208mD Neu", 0f, 19, 14005, (short)311, 30 },
                    { 312, "HDB1208mD I", 1f, 1, 14030, (short)312, 30 },
                    { 313, "HDB1208mD D", 2f, 2, 14002, (short)313, 30 },
                    { 321, "HDB1208D Neu", 0f, 19, 13005, (short)321, 31 },
                    { 322, "HDB1208D I", 1f, 1, 13030, (short)322, 31 },
                    { 323, "HDB1208D D", 2f, 2, 13002, (short)23, 31 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LoadCarriers",
                keyColumn: "Id",
                keyValue: 311);

            migrationBuilder.DeleteData(
                table: "LoadCarriers",
                keyColumn: "Id",
                keyValue: 312);

            migrationBuilder.DeleteData(
                table: "LoadCarriers",
                keyColumn: "Id",
                keyValue: 313);

            migrationBuilder.DeleteData(
                table: "LoadCarriers",
                keyColumn: "Id",
                keyValue: 321);

            migrationBuilder.DeleteData(
                table: "LoadCarriers",
                keyColumn: "Id",
                keyValue: 322);

            migrationBuilder.DeleteData(
                table: "LoadCarriers",
                keyColumn: "Id",
                keyValue: 323);
        }
    }
}
