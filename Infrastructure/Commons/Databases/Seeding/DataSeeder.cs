using Application.Commons.Options;
using Domain.Users.Root;
using Infrastructure.Commons.Databases.Seeding.Locations;
using Infrastructure.Commons.Databases.Seeding.Users;
using Infrastructure.Commons.Loaders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Commons.Databases.Seeding;

internal class DataSeeder {
    readonly SeedingOptions _options;
    readonly UserManager<User> _userManager;
    readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public DataSeeder(IServiceProvider services) {
        _options =
            services.GetRequiredService<IOptions<SeedingOptions>>().Value
            ?? throw new Exception("Seeding options are null");
        _userManager = services.GetRequiredService<UserManager<User>>();
        _roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    }

    public async Task Seed(TripDbContext context) {
        bool hasNoRegionsInDb = !await context.Regions.AnyAsync();
        if (hasNoRegionsInDb) {
            Console.WriteLine("Seeding regions");
            await new InsertRegionsAsync(DataSeed.Regions).Seed(context);
        }

        var hasNoPeaksInDb = !await context.Peaks.AnyAsync();
        if (hasNoPeaksInDb) {
            Console.WriteLine("Seeding Peaks");

            var resourcePath = _options.PeaksUrl;
            Console.WriteLine("Seeding from: " + resourcePath);

            await new InsertMountainPeaks(await PeakCsvLoader.LoadFromLink(resourcePath)).Seed(
                context
            );
        }

        await new InsertRoles(_roleManager).Seed(context);
        await new InsertBaseUser(_userManager).Seed(context);
        await new InsertAdminUser(_userManager, _options).Seed(context);
    }
}
