using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLegalCategoryForeignKeyToActMasterSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, seed the migration history for migrations that have already been applied
            // This prevents EF Core from trying to apply old migrations to an existing database
            var existingMigrations = new[]
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

            foreach (var migrationId in existingMigrations)
            {
                migrationBuilder.Sql($@"
                    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
                    SELECT '{migrationId}', '10.0.9'
                    WHERE NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '{migrationId}')
                ");
            }

            // Add LegalCategoryId column if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME='ActMasters' AND COLUMN_NAME='LegalCategoryId')
                BEGIN
                    ALTER TABLE ActMasters ADD LegalCategoryId INT NULL;
                END
            ");

            // Create index on LegalCategoryId if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes 
                WHERE name='IX_ActMasters_LegalCategoryId' AND object_id=OBJECT_ID('ActMasters'))
                BEGIN
                    CREATE INDEX IX_ActMasters_LegalCategoryId ON ActMasters(LegalCategoryId);
                END
            ");

            // Add foreign key constraint if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
                WHERE CONSTRAINT_NAME='FK_ActMasters_LegalCategoryMasters_LegalCategoryId')
                BEGIN
                    ALTER TABLE ActMasters 
                    ADD CONSTRAINT FK_ActMasters_LegalCategoryMasters_LegalCategoryId 
                    FOREIGN KEY (LegalCategoryId) REFERENCES LegalCategoryMasters(Id)
                    ON DELETE SET NULL;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
                WHERE CONSTRAINT_NAME='FK_ActMasters_LegalCategoryMasters_LegalCategoryId')
                BEGIN
                    ALTER TABLE ActMasters 
                    DROP CONSTRAINT FK_ActMasters_LegalCategoryMasters_LegalCategoryId;
                END
            ");

            // Drop index if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes 
                WHERE name='IX_ActMasters_LegalCategoryId' AND object_id=OBJECT_ID('ActMasters'))
                BEGIN
                    DROP INDEX IX_ActMasters_LegalCategoryId ON ActMasters;
                END
            ");

            // Drop column if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME='ActMasters' AND COLUMN_NAME='LegalCategoryId')
                BEGIN
                    ALTER TABLE ActMasters DROP COLUMN LegalCategoryId;
                END
            ");
        }
    }
}
