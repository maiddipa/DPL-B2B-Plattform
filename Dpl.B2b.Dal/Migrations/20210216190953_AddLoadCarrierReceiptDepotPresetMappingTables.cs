using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddLoadCarrierReceiptDepotPresetMappingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_LoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPresetId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadCarriers_LoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers");

            migrationBuilder.DropIndex(
                name: "IX_LoadCarriers_LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_LoadCarrierReceiptDepotPresetId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers");

            migrationBuilder.DropColumn(
                name: "LoadCarrierReceiptDepotPresetId",
                table: "Customers");

            migrationBuilder.CreateTable(
                name: "CustomerLoadCarrierReceiptDepotPreset",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false),
                    LoadCarrierReceiptDepotPresetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLoadCarrierReceiptDepotPreset", x => new { x.CustomerId, x.LoadCarrierReceiptDepotPresetId });
                    table.ForeignKey(
                        name: "FK_CustomerLoadCarrierReceiptDepotPreset_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerLoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPresetId",
                        column: x => x.LoadCarrierReceiptDepotPresetId,
                        principalTable: "LoadCarrierReceiptDepotPreset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadCarrierReceiptDepotPresetLoadCarrier",
                columns: table => new
                {
                    LoadCarrierId = table.Column<int>(nullable: false),
                    LoadCarrierReceiptDepotPresetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierReceiptDepotPresetLoadCarrier", x => new { x.LoadCarrierReceiptDepotPresetId, x.LoadCarrierId });
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceiptDepotPresetLoadCarrier_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceiptDepotPresetLoadCarrier_LoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPresetId",
                        column: x => x.LoadCarrierReceiptDepotPresetId,
                        principalTable: "LoadCarrierReceiptDepotPreset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 111,
                column: "Name",
                value: "PermissionResourceType");

            migrationBuilder.InsertData(
                table: "LocalizationItems",
                columns: new[] { "Id", "FieldName", "Name", "Reference", "Type" },
                values: new object[] { 129, null, "BusinessHourExceptionType", "Dpl.B2b.Common.Enumerations.BusinessHourExceptionType", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPresetId",
                table: "CustomerLoadCarrierReceiptDepotPreset",
                column: "LoadCarrierReceiptDepotPresetId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceiptDepotPresetLoadCarrier_LoadCarrierId",
                table: "LoadCarrierReceiptDepotPresetLoadCarrier",
                column: "LoadCarrierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerLoadCarrierReceiptDepotPreset");

            migrationBuilder.DropTable(
                name: "LoadCarrierReceiptDepotPresetLoadCarrier");

            migrationBuilder.DeleteData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.AddColumn<int>(
                name: "LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoadCarrierReceiptDepotPresetId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 111,
                column: "Name",
                value: "ResourceType");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarriers_LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers",
                column: "LoadCarrierReceiptDepotPresetId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LoadCarrierReceiptDepotPresetId",
                table: "Customers",
                column: "LoadCarrierReceiptDepotPresetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_LoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPresetId",
                table: "Customers",
                column: "LoadCarrierReceiptDepotPresetId",
                principalTable: "LoadCarrierReceiptDepotPreset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarriers_LoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers",
                column: "LoadCarrierReceiptDepotPresetId",
                principalTable: "LoadCarrierReceiptDepotPreset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
