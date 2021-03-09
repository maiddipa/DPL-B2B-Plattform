using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class FixUserGroupRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUserGroupRelations_Users_ChangedById",
                table: "UserUserGroupRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUserGroupRelations_Users_CreatedById",
                table: "UserUserGroupRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUserGroupRelations_Users_DeletedById",
                table: "UserUserGroupRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUserGroupRelations_Users_UserId",
                table: "UserUserGroupRelations");

            migrationBuilder.DropIndex(
                name: "IX_UserUserGroupRelations_ChangedById",
                table: "UserUserGroupRelations");

            migrationBuilder.DropIndex(
                name: "IX_UserUserGroupRelations_CreatedById",
                table: "UserUserGroupRelations");

            migrationBuilder.DropIndex(
                name: "IX_UserUserGroupRelations_DeletedById",
                table: "UserUserGroupRelations");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "UserUserGroupRelations");

            migrationBuilder.DropColumn(
                name: "ChangedById",
                table: "UserUserGroupRelations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserUserGroupRelations");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserUserGroupRelations");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "UserUserGroupRelations");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "UserUserGroupRelations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserUserGroupRelations");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserGroupRelations_Users_UserId",
                table: "UserUserGroupRelations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUserGroupRelations_Users_UserId",
                table: "UserUserGroupRelations");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "UserUserGroupRelations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChangedById",
                table: "UserUserGroupRelations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserUserGroupRelations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "UserUserGroupRelations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "UserUserGroupRelations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                table: "UserUserGroupRelations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserUserGroupRelations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserUserGroupRelations_ChangedById",
                table: "UserUserGroupRelations",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserUserGroupRelations_CreatedById",
                table: "UserUserGroupRelations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserUserGroupRelations_DeletedById",
                table: "UserUserGroupRelations",
                column: "DeletedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserGroupRelations_Users_ChangedById",
                table: "UserUserGroupRelations",
                column: "ChangedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserGroupRelations_Users_CreatedById",
                table: "UserUserGroupRelations",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserGroupRelations_Users_DeletedById",
                table: "UserUserGroupRelations",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserGroupRelations_Users_UserId",
                table: "UserUserGroupRelations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
