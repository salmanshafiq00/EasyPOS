using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPOS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedAppPageField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPageActions");

            migrationBuilder.DropIndex(
                name: "IX_AppMenus_Label",
                table: "AppMenus");

            migrationBuilder.CreateIndex(
                name: "IX_AppMenus_Label",
                table: "AppMenus",
                column: "Label");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppMenus_Label",
                table: "AppMenus");

            migrationBuilder.CreateTable(
                name: "AppPageActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionTypeId = table.Column<int>(type: "int", nullable: true),
                    ApplicationPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FunctionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    NavigationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowCaptionDevice = table.Column<bool>(type: "bit", nullable: true),
                    ShowCaptionWeb = table.Column<bool>(type: "bit", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPageActions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppMenus_Label",
                table: "AppMenus",
                column: "Label",
                unique: true);
        }
    }
}
