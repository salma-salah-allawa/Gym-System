using GymSystemApp.Contexts;
using GymSystemApp.DAL.DataSeeds;
using GymSystemApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymSystemApp.PL
{
    public static class ProgramExtentions
    {
        public static async Task MigrationAndSeedAsync(this WebApplication app)
        {
            using var scope= app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GymAppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var configrations = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var roleManager= scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager= scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var pendingMigration = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigration.Any())
            {
                logger.LogInformation($"Apply {pendingMigration.Count()} Pending Migrations....");
                await context.Database.MigrateAsync();
            }
            var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");

            await GymDataSeed.SeedAsync(context, seedPath,logger);
            await IdentityDataSeed.SeedAsync(roleManager, userManager, logger);
        }
    }
}
