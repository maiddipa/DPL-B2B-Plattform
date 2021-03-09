using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class CombineMigrationsForLoadCarrierReceiptBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSortingRequired",
                table: "PostingRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSortingCompleted",
                table: "LoadCarrierReceipts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSortingRequired",
                table: "LoadCarrierReceipts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "LoadCarrierSortings",
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
                    LoadCarrierReceiptId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierSortings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCarrierSortings_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierSortings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierSortings_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierSortings_LoadCarrierReceipts_LoadCarrierReceiptId",
                        column: x => x.LoadCarrierReceiptId,
                        principalTable: "LoadCarrierReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadCarrierSortingResult",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadCarrierSortingId = table.Column<int>(nullable: false),
                    LoadCarrierId = table.Column<int>(nullable: false),
                    InputQuantity = table.Column<int>(nullable: false),
                    RemainingQuantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierSortingResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCarrierSortingResult_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierSortingResult_LoadCarrierSortings_LoadCarrierSortingId",
                        column: x => x.LoadCarrierSortingId,
                        principalTable: "LoadCarrierSortings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadCarrierSortingResultOutput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadCarrierSortingResultId = table.Column<int>(nullable: false),
                    LoadCarrierQuantity = table.Column<int>(nullable: false),
                    LoadCarrierId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierSortingResultOutput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCarrierSortingResultOutput_LoadCarriers_LoadCarrierId",
                        column: x => x.LoadCarrierId,
                        principalTable: "LoadCarriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadCarrierSortingResultOutput_LoadCarrierSortingResult_LoadCarrierSortingResultId",
                        column: x => x.LoadCarrierSortingResultId,
                        principalTable: "LoadCarrierSortingResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "LocalizationItems",
                columns: new[] { "Id", "FieldName", "Name", "Reference", "Type" },
                values: new object[] { 127, null, "LoadCarrierReceiptDepoPresetCategory", "Dpl.B2b.Common.Enumerations.LoadCarrierReceiptDepotPresetCategory", 1 });

            migrationBuilder.InsertData(
                table: "LocalizationItems",
                columns: new[] { "Id", "FieldName", "Name", "Reference", "Type" },
                values: new object[] { 128, null, "ResourceAction", "Dpl.B2b.Common.Enumerations.ResourceAction", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierSortingResult_LoadCarrierId",
                table: "LoadCarrierSortingResult",
                column: "LoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierSortingResult_LoadCarrierSortingId",
                table: "LoadCarrierSortingResult",
                column: "LoadCarrierSortingId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierSortingResultOutput_LoadCarrierId",
                table: "LoadCarrierSortingResultOutput",
                column: "LoadCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierSortingResultOutput_LoadCarrierSortingResultId",
                table: "LoadCarrierSortingResultOutput",
                column: "LoadCarrierSortingResultId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierSortings_ChangedById",
                table: "LoadCarrierSortings",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierSortings_CreatedById",
                table: "LoadCarrierSortings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierSortings_DeletedById",
                table: "LoadCarrierSortings",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierSortings_LoadCarrierReceiptId",
                table: "LoadCarrierSortings",
                column: "LoadCarrierReceiptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadCarrierSortingResultOutput");

            migrationBuilder.DropTable(
                name: "LoadCarrierSortingResult");

            migrationBuilder.DropTable(
                name: "LoadCarrierSortings");

            migrationBuilder.DeleteData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DropColumn(
                name: "IsSortingRequired",
                table: "PostingRequests");

            migrationBuilder.DropColumn(
                name: "IsSortingCompleted",
                table: "LoadCarrierReceipts");

            migrationBuilder.DropColumn(
                name: "IsSortingRequired",
                table: "LoadCarrierReceipts");
        }
    }
}
