using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPOS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AppPageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Suppliers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CompanyInfos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "CompanyInfos");
        }
    }
}
