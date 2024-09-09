using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPOS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AppPageEntityChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPageField");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "Permission",
                table: "AppPages");

            migrationBuilder.RenameColumn(
                name: "RouterLink",
                table: "AppPages",
                newName: "ComponentName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ComponentName",
                table: "AppPages",
                newName: "RouterLink");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AppPages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppPageField",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BgColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DSName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DbField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableLink = table.Column<bool>(type: "bit", nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilterType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFilterable = table.Column<bool>(type: "bit", nullable: false),
                    IsGlobalFilterable = table.Column<bool>(type: "bit", nullable: false),
                    IsSortable = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    LinkBaseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkValueFieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    TextAlign = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPageField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPageField_AppPages_AppPageId",
                        column: x => x.AppPageId,
                        principalTable: "AppPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppPageField_AppPageId",
                table: "AppPageField",
                column: "AppPageId");
        }
    }
}
