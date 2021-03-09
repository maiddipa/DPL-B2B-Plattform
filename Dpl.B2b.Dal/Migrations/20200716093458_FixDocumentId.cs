using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class FixDocumentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE [dbo].[Vouchers]
                SET [DocumentId] = D.Id 
                FROM Vouchers V INNER JOIN Documents D ON D.VoucherId = V.ID
                WHERE V.DocumentId = 0
            ");
            migrationBuilder.Sql(@"
                UPDATE [dbo].[LoadCarrierReceipts]
                SET [DocumentId] = D.Id 
                FROM LoadCarrierReceipts U INNER JOIN Documents D ON D.VoucherId = U.ID
                WHERE U.DocumentId = 0
            ");
            migrationBuilder.Sql(@"
                UPDATE [dbo].[OrderMatches]
                SET [DocumentId] = D.Id 
                FROM OrderMatches U INNER JOIN Documents D ON D.VoucherId = U.ID
                WHERE U.DocumentId = 0
            ");
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_LoadCarrierReceipts_LoadCarrierReceiptId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_OrderMatches_OrderMatchId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Vouchers_VoucherId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_LoadCarrierReceiptId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_OrderMatchId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_VoucherId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LoadCarrierReceiptId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "OrderMatchId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "Documents");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_DocumentId",
                table: "Vouchers",
                column: "DocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderMatches_DocumentId",
                table: "OrderMatches",
                column: "DocumentId",
                unique: true,
                filter: "[DocumentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCarrierReceipts_DocumentId",
                table: "LoadCarrierReceipts",
                column: "DocumentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadCarrierReceipts_Documents_DocumentId",
                table: "LoadCarrierReceipts",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderMatches_Documents_DocumentId",
                table: "OrderMatches",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Documents_DocumentId",
                table: "Vouchers",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadCarrierReceipts_Documents_DocumentId",
                table: "LoadCarrierReceipts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderMatches_Documents_DocumentId",
                table: "OrderMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Documents_DocumentId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_DocumentId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_OrderMatches_DocumentId",
                table: "OrderMatches");

            migrationBuilder.DropIndex(
                name: "IX_LoadCarrierReceipts_DocumentId",
                table: "LoadCarrierReceipts");

            migrationBuilder.AddColumn<int>(
                name: "LoadCarrierReceiptId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderMatchId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoucherId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_LoadCarrierReceiptId",
                table: "Documents",
                column: "LoadCarrierReceiptId",
                unique: true,
                filter: "[LoadCarrierReceiptId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_OrderMatchId",
                table: "Documents",
                column: "OrderMatchId",
                unique: true,
                filter: "[OrderMatchId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_VoucherId",
                table: "Documents",
                column: "VoucherId",
                unique: true,
                filter: "[VoucherId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_LoadCarrierReceipts_LoadCarrierReceiptId",
                table: "Documents",
                column: "LoadCarrierReceiptId",
                principalTable: "LoadCarrierReceipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_OrderMatches_OrderMatchId",
                table: "Documents",
                column: "OrderMatchId",
                principalTable: "OrderMatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Vouchers_VoucherId",
                table: "Documents",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.Sql(@"
                UPDATE [dbo].[Documents]
                SET [VoucherId] = V.Id 
                FROM Vouchers V INNER JOIN Documents D ON D.Id = V.DocumentId
            ");
            migrationBuilder.Sql(@"
                UPDATE [dbo].[Documents]
                SET [LoadCarrierReceiptId] = L.Id 
                FROM LoadCarrierReceipts L INNER JOIN Documents D ON D.Id = L.DocumentId
            ");
            migrationBuilder.Sql(@"
                UPDATE [dbo].[Documents]
                SET [OrderMatchId] = O.Id 
                FROM OrderMatches O INNER JOIN Documents D ON D.Id = O.DocumentId
            ");
        }
    }
}
