using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class AddCustomDocumentTemplateLabelEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "DocumentTemplateLabel");

            migrationBuilder.CreateTable(
                name: "CustomDocumentLabel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: true),
                    DocumentTemplateId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    ReportLabel = table.Column<string>(maxLength: 255, nullable: true),
                    UiLabel = table.Column<string>(maxLength: 255, nullable: true),
                    Text = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomDocumentLabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomDocumentLabel_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomDocumentLabel_CustomerId",
                table: "CustomDocumentLabel",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomDocumentLabel");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "DocumentTemplateLabel",
                type: "int",
                nullable: true);
        }
    }
}
