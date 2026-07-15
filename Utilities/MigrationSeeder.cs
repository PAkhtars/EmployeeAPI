using EmployeeAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeeAPI.Utilities
{
    public class MigrationSeeder
    {
        public static async Task SeedMigrationHistory(AppDbContext context)
        {
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

            foreach (var migration in migrations)
            {
                var sql = $@"
                    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
                    SELECT '{migration}', '10.0.9'
                    WHERE NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '{migration}')
                ";
                
                await context.Database.ExecuteSqlRawAsync(sql);
            }

            await context.SaveChangesAsync();
        }
    }
}
