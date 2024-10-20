using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPOS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removecountryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
