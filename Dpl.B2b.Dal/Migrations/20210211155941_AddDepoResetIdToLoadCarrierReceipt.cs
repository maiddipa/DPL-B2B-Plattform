using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddDepoResetIdToLoadCarrierReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepoPresetId",
                table: "LoadCarrierReceipts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepotPresetId",
                table: "LoadCarrierReceipts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoadCarrierReceiptDepotPresetId",
                table: "Customers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoadCarrierReceiptDepotPreset",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ChangedById = table.Column<int>(nullable: true),
                    ChangedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    IsSortingRequired = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierReceiptDepotPreset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceiptDepotPreset_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceiptDepotPreset_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierReceiptDepotPreset_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarriers_LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers",
                column: "LoadCarrierReceiptDepotPresetId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_DepotPresetId",
                table: "LoadCarrierReceipts",
                column: "DepotPresetId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LoadCarrierReceiptDepotPresetId",
                table: "Customers",
                column: "LoadCarrierReceiptDepotPresetId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceiptDepotPreset_ChangedById",
                table: "LoadCarrierReceiptDepotPreset",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceiptDepotPreset_CreatedById",
                table: "LoadCarrierReceiptDepotPreset",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceiptDepotPreset_DeletedById",
                table: "LoadCarrierReceiptDepotPreset",
                column: "DeletedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_LoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPresetId",
                table: "Customers",
                column: "LoadCarrierReceiptDepotPresetId",
                principalTable: "LoadCarrierReceiptDepotPreset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarrierReceipts_LoadCarrierReceiptDepotPreset_DepotPresetId",
                table: "LoadCarrierReceipts",
                column: "DepotPresetId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_LoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPresetId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadCarrierReceipts_LoadCarrierReceiptDepotPreset_DepotPresetId",
                table: "LoadCarrierReceipts");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadCarriers_LoadCarrierReceiptDepotPreset_LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers");

            migrationBuilder.DropTable(
                name: "LoadCarrierReceiptDepotPreset");

            migrationBuilder.DropIndex(
                name: "IX_LoadCarriers_LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers");

            migrationBuilder.DropIndex(
                name: "IX_LoadCarrierReceipts_DepotPresetId",
                table: "LoadCarrierReceipts");

            migrationBuilder.DropIndex(
                name: "IX_Customers_LoadCarrierReceiptDepotPresetId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LoadCarrierReceiptDepotPresetId",
                table: "LoadCarriers");

            migrationBuilder.DropColumn(
                name: "DepoPresetId",
                table: "LoadCarrierReceipts");

            migrationBuilder.DropColumn(
                name: "DepotPresetId",
                table: "LoadCarrierReceipts");

            migrationBuilder.DropColumn(
                name: "LoadCarrierReceiptDepotPresetId",
                table: "Customers");
        }
    }
}
