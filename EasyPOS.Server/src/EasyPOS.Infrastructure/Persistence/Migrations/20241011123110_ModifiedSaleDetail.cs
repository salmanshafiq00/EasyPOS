using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPOS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedSaleDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "SaleDetails",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "SaleDetails",
                newName: "ProductUnitPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "SaleDetails",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "ProductUnitPrice",
                table: "SaleDetails",
                newName: "SubTotal");
        }
    }
}
