using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class ExpressCodeUsageCondition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpressCodeUsageCondition",
                columns: table => new
                {
                    PostingAccountId = table.Column<int>(nullable: false),
                    AllowDropOff = table.Column<bool>(nullable: false),
                    AllowReceivingVoucher = table.Column<bool>(nullable: false),
                    AcceptForDropOff = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpressCodeUsageCondition", x => x.PostingAccountId);
                    table.ForeignKey(
                        name: "FK_ExpressCodeUsageCondition_PostingAccounts_PostingAccountId",
                        column: x => x.PostingAccountId,
                        principalTable: "PostingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpressCodeUsageCondition");
        }
    }
}
