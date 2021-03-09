using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddRefLtmsAccountNumberToPa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 30000);

            migrationBuilder.DeleteData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 30001);

            migrationBuilder.DeleteData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 30002);

            migrationBuilder.DeleteData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 30003);

            migrationBuilder.DeleteData(
                table: "LocalizationItems",
                keyColumn: "Id",
                keyValue: 40000);

            migrationBuilder.AddColumn<string>(
                name: "RefLtmsAccountNumber",
                table: "PostingAccounts",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefLtmsAccountNumber",
                table: "PostingAccounts");

            migrationBuilder.InsertData(
                table: "LocalizationItems",
                columns: new[] { "Id", "FieldName", "Name", "Reference", "Type" },
                values: new object[,]
                {
                    { 30000, null, "NotAllowedByRule", "Dpl.B2b.Contracts.Rules.Messages.Errors.Common.NotAllowedByRule", 2 },
                    { 30001, null, "OrderGroupCancel", "Dpl.B2b.Contracts.Rules.Messages.Errors.OrderGroup.OrderGroupCancel", 2 },
                    { 30002, null, "OrderGroupHasOpenOrConfirmedOrders", "Dpl.B2b.Contracts.Rules.Messages.Errors.OrderGroup.OrderGroupHasOpenOrConfirmedOrders", 2 },
                    { 30003, null, "OrderGroupHasOrders", "Dpl.B2b.Contracts.Rules.Messages.Errors.OrderGroup.OrderGroupHasOrders", 2 },
                    { 40000, null, "OrderGroupHasOrders", "Dpl.B2b.Contracts.Rules.Messages.Warnings.Common.NoGuarantee", 3 }
                });
        }
    }
}
