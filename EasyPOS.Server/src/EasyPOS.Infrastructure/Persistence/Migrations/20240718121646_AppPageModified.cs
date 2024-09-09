using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPOS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AppPageModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowFilter",
                table: "AppPageField");

            migrationBuilder.DropColumn(
                name: "AllowSort",
                table: "AppPageField");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "AppPageField",
                newName: "DbField");

            migrationBuilder.RenameColumn(
                name: "CellTemplate",
                table: "AppPageField",
                newName: "DSName");

            migrationBuilder.AlterColumn<bool>(
                name: "IsVisible",
                table: "AppPageField",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AppPageField",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFilterable",
                table: "AppPageField",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGlobalFilterable",
                table: "AppPageField",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSortable",
                table: "AppPageField",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFilterable",
                table: "AppPageField");

            migrationBuilder.DropColumn(
                name: "IsGlobalFilterable",
                table: "AppPageField");

            migrationBuilder.DropColumn(
                name: "IsSortable",
                table: "AppPageField");

            migrationBuilder.RenameColumn(
                name: "DbField",
                table: "AppPageField",
                newName: "Position");

            migrationBuilder.RenameColumn(
                name: "DSName",
                table: "AppPageField",
                newName: "CellTemplate");

            migrationBuilder.AlterColumn<bool>(
                name: "IsVisible",
                table: "AppPageField",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AppPageField",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "AllowFilter",
                table: "AppPageField",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowSort",
                table: "AppPageField",
                type: "bit",
                nullable: true);
        }
    }
}
