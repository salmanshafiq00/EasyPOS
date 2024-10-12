using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPOS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CombinationUniqueKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LookupDetails_Name",
                table: "LookupDetails");

            migrationBuilder.CreateIndex(
                name: "IX_LookupDetails_LookupId_Name",
                table: "LookupDetails",
                columns: new[] { "LookupId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LookupDetails_LookupId_Name",
                table: "LookupDetails");

            migrationBuilder.CreateIndex(
                name: "IX_LookupDetails_Name",
                table: "LookupDetails",
                column: "Name",
                unique: true);
        }
    }
}
