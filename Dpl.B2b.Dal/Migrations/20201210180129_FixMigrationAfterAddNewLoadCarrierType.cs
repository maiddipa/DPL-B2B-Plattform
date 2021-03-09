using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class FixMigrationAfterAddNewLoadCarrierType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LoadCarrierTypes",
                keyColumn: "Id",
                keyValue: 21,
                column: "RefLmsLoadCarrierTypeId",
                value: 15);

            migrationBuilder.InsertData(
                table: "LoadCarrierTypes",
                columns: new[] { "Id", "BaseLoadCarrier", "Description", "MaxStackHeight", "MaxStackHeightJumbo", "Name", "Order", "QuantityPerEur", "RefLmsLoadCarrierTypeId", "RefLtmsArticleId" },
                values: new object[,]
                {
                    { 30, 0, null, 8, 8, "HDB1208mD", 0f, 1, 14, (short)30 },
                    { 31, 0, null, 0, 0, "HDB1208D", 0f, 1, 13, (short)31 }
                });

            migrationBuilder.UpdateData(
                table: "LoadCarriers",
                keyColumn: "Id",
                keyValue: 151,
                column: "RefLmsQuality2PalletId",
                value: 15005);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LoadCarrierTypes",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "LoadCarrierTypes",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.UpdateData(
                table: "LoadCarrierTypes",
                keyColumn: "Id",
                keyValue: 21,
                column: "RefLmsLoadCarrierTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "LoadCarriers",
                keyColumn: "Id",
                keyValue: 151,
                column: "RefLmsQuality2PalletId",
                value: 0);
        }
    }
}
