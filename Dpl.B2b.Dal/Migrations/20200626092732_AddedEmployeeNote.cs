using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddedEmployeeNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DplNoteId",
                table: "PostingRequests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeNote",
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
                    Reason = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Contact = table.Column<string>(maxLength: 255, nullable: true),
                    ContactedAt = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(maxLength: 255, nullable: true),
                    LoadCarrierReceiptId = table.Column<int>(nullable: true),
                    OrderId = table.Column<int>(nullable: true),
                    VoucherId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeNote_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeNote_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeNote_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeNote_LoadCarrierReceipts_LoadCarrierReceiptId",
                        column: x => x.LoadCarrierReceiptId,
                        principalTable: "LoadCarrierReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeNote_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeNote_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeNote_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostingRequests_DplNoteId",
                table: "PostingRequests",
                column: "DplNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeNote_ChangedById",
                table: "EmployeeNote",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeNote_CreatedById",
                table: "EmployeeNote",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeNote_DeletedById",
                table: "EmployeeNote",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeNote_LoadCarrierReceiptId",
                table: "EmployeeNote",
                column: "LoadCarrierReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeNote_OrderId",
                table: "EmployeeNote",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeNote_UserId",
                table: "EmployeeNote",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeNote_VoucherId",
                table: "EmployeeNote",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostingRequests_EmployeeNote_DplNoteId",
                table: "PostingRequests",
                column: "DplNoteId",
                principalTable: "EmployeeNote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostingRequests_EmployeeNote_DplNoteId",
                table: "PostingRequests");

            migrationBuilder.DropTable(
                name: "EmployeeNote");

            migrationBuilder.DropIndex(
                name: "IX_PostingRequests_DplNoteId",
                table: "PostingRequests");

            migrationBuilder.DropColumn(
                name: "DplNoteId",
                table: "PostingRequests");
        }
    }
}
