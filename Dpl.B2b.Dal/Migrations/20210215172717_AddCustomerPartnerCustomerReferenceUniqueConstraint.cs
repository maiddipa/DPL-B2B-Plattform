using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddCustomerPartnerCustomerReferenceUniqueConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // include directory id in customer partner reference
            migrationBuilder.Sql(@"
UPDATE CP
SET CP.CustomerReference = CONCAT(CPDA.DirectoryId, '||', CP.CustomerReference)
FROM CustomerPartners CP
JOIN CustomerPartnerDirectoryAccesses CPDA ON CPDA.CustomerPartnerId = CP.Id
WHERE CP.CustomerReference IS NOT NULL
");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPartners_CustomerReference",
                table: "CustomerPartners",
                column: "CustomerReference",
                unique: true,
                filter: "[CustomerReference] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerPartners_CustomerReference",
                table: "CustomerPartners");
        }
    }
}
