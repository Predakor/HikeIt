using Domain.Users;
using Infrastructure.Data.Loaders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Seeding;

internal static class DataSeeder {
    static async Task Seed(TripDbContext dbContext, IServiceScope scope) {
        bool hasNoRegionsInDb = !await dbContext.Regions.AnyAsync();
        if (hasNoRegionsInDb) {
            Console.WriteLine("Seeding regions");
            await new InsertRegionsAsync(DataSeed.Regions).Seed(dbContext);
        }

        var hasNoPeaksInDb = !await dbContext.Peaks.AnyAsync();
        if (hasNoPeaksInDb) {
            Console.WriteLine("Seeding Peaks");

            var resourcePath = Path.Combine(AppContext.BaseDirectory, "peaks.csv");
            Console.WriteLine("Path for seeding source: " + resourcePath);

            await new InsertMountainPeaks(PeakCsvLoader.LoadFrom(resourcePath)).Seed(dbContext);
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var demoUser = User.DemoUser;
        var hasNoDemoUser = (await userManager.FindByNameAsync(demoUser.UserName!)) == null;
        if (hasNoDemoUser) {
            Console.WriteLine("Seeding default users");
            await new InsertBaseUser(userManager, demoUser).Seed(dbContext);
        }
    }

    public static async Task TrySeeding(TripDbContext dbContext, IServiceProvider services) {
        try {
            using var scope = services.CreateScope();

            await Seed(dbContext, scope);

            await dbContext.SaveChangesAsync();
        }
        catch (Exception exc) {
            Console.WriteLine($"Seeding failed reason  {exc.Message}");
            throw;
        }
    }
}
