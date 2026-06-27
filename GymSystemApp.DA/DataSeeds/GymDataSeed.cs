using GymSystemApp.Contexts;
using GymSystemApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GymSystemApp.DAL.DataSeeds
{
    public static class GymDataSeed
    {
        public static async Task SeedAsync(GymAppDbContext context, string FilePath, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if (!await context.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json", FilePath);
                    if (plans.Count() > 0)
                    {
                        await context.Plans.AddRangeAsync(plans, ct);
                        logger.LogInformation($"Seeded {plans.Count} Plans successfully.");
                    }
                }
                if (context.ChangeTracker.HasChanges())
                {
                    await context.SaveChangesAsync(ct);
                    logger.LogInformation("Saved changes to the database after seeding.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to Seed Gym Data.");
                throw;
            }
        }
        private static List<T> LoadDataFromJsonFile<T>(string fileName, string folderPath)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if(!File.Exists(filePath))
                    throw new FileNotFoundException($"The file '{filePath}' was not found for data Seeding.");

            var data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }
    }
}
