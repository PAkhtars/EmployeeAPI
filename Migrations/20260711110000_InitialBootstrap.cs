using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialBootstrap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This migration only seeds the __EFMigrationsHistory table for migrations
            // that have already been applied to the database.
            // This is needed when the database schema exists but the migration history table is empty.

            migrationBuilder.Sql("SET IDENTITY_INSERT __EFMigrationsHistory ON;", suppressTransaction: true);

            var migrations = new[]
            {
                "20260521081212_InitialCreate",
                "20260605121524_FixPollOptionKey",
                "20260609104148_AddVoteTable",
                "20260609125930_InitialCreate1",
                "20260629093303_ActMaster",
                "20260629112002_ActDetails",
                "20260709122455_EmailSending",
                "20260710135051_Category Master",
                "20260711045631_AddIconClassToLegalCategoryMaster"
            };

            int order = 1;
            foreach (var migrationId in migrations)
            {
                migrationBuilder.Sql($@"
                    IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '{migrationId}')
                    BEGIN
                        INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
                        VALUES ('{migrationId}', '10.0.9');
                    END
                ");
                order++;
            }

            migrationBuilder.Sql("SET IDENTITY_INSERT __EFMigrationsHistory OFF;", suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // This is a bootstrap migration and should not be rolled back
        }
    }
}
