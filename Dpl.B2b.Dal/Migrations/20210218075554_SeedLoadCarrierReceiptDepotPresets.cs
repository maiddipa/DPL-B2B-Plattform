using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class SeedLoadCarrierReceiptDepotPresets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LoadCarrierReceiptDepotPreset",
                columns: new[] { "Id", "Category", "ChangedAt", "ChangedById", "CreatedAt", "CreatedById", "DeletedAt", "DeletedById", "IsDeleted", "IsSortingRequired", "Name" },
                values: new object[,]
                {
                    { 1, 0, null, null, null, null, null, null, false, true, "[EUR und DD]: Depotannahme (von Dritten)" },
                    { 2, 0, null, null, null, null, null, null, false, true, "[H1]: Depotannahme (von Dritten)" },
                    { 3, 1, null, null, null, null, null, null, false, false, "[EUR und DD]:  Einlagerung (für DPL)" },
                    { 4, 1, null, null, null, null, null, null, false, false, "[H1]:  Einlagerung (für DPL)" },
                    { 5, 1, null, null, null, null, null, null, false, false, "[H1 und E2KR]:  Einlagerung (für DPL)" },
                    { 6, 1, null, null, null, null, null, null, false, false, "[H1 und E2EP]:  Einlagerung (für DPL)" },
                    { 7, 1, null, null, null, null, null, null, false, false, "[H1 und E1EP]:  Einlagerung (für DPL)" },
                    { 8, 1, null, null, null, null, null, null, false, false, "[H1 und E1KR]:  Einlagerung (für DPL)" }
                });

            migrationBuilder.InsertData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                columns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                values: new object[,]
                {
                    { 1, 103 },
                    { 8, 410 },
                    { 8, 406 },
                    { 7, 903 },
                    { 7, 902 },
                    { 7, 410 },
                    { 7, 406 },
                    { 6, 603 },
                    { 6, 602 },
                    { 6, 410 },
                    { 6, 406 },
                    { 5, 703 },
                    { 5, 702 },
                    { 5, 410 },
                    { 5, 406 },
                    { 4, 410 },
                    { 4, 406 },
                    { 3, 208 },
                    { 3, 204 },
                    { 3, 203 },
                    { 3, 104 },
                    { 3, 103 },
                    { 2, 410 },
                    { 2, 406 },
                    { 1, 208 },
                    { 1, 204 },
                    { 1, 203 },
                    { 1, 104 },
                    { 8, 802 },
                    { 8, 803 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 1, 103 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 1, 104 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 1, 203 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 1, 204 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 1, 208 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 2, 406 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 2, 410 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 3, 103 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 3, 104 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 3, 203 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 3, 204 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 3, 208 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 4, 406 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 4, 410 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 5, 406 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 5, 410 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 5, 702 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 5, 703 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 6, 406 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 6, 410 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 6, 602 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 6, 603 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 7, 406 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 7, 410 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 7, 902 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 7, 903 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 8, 406 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 8, 410 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 8, 802 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                keyColumns: new[] { "LoadCarrierReceiptDepotPresetId", "LoadCarrierId" },
                keyValues: new object[] { 8, 803 });

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPreset",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPreset",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPreset",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPreset",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPreset",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPreset",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPreset",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LoadCarrierReceiptDepotPreset",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
