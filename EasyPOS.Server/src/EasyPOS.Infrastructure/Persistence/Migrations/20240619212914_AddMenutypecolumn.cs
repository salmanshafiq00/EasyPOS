using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPOS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMenutypecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MenuTypeId",
                table: "AppMenus",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppMenus_MenuTypeId",
                table: "AppMenus",
                column: "MenuTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppMenus_LookupDetails_MenuTypeId",
                table: "AppMenus",
                column: "MenuTypeId",
                principalTable: "LookupDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppMenus_LookupDetails_MenuTypeId",
                table: "AppMenus");

            migrationBuilder.DropIndex(
                name: "IX_AppMenus_MenuTypeId",
                table: "AppMenus");

            migrationBuilder.DropColumn(
                name: "MenuTypeId",
                table: "AppMenus");
        }
    }
}
