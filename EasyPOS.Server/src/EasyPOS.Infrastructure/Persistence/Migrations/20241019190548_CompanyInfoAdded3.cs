using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPOS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CompanyInfoAdded3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a filtered unique index on the nullable int column DevCode
            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX IX_Lookups_DevCode
                      ON dbo.Lookups (DevCode)
                      WHERE DevCode IS NOT NULL;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the index if rolling back the migration
            migrationBuilder.Sql(
                @"DROP INDEX IX_Lookups_DevCode ON dbo.Lookups;"
            );
        }
    }
}
