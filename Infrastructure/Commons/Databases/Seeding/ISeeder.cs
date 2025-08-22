using Infrastructure.Commons.Databases;

namespace Infrastructure.Commons.Databases.Seeding;

public interface ISeeder {
    Task Seed(TripDbContext dbContext);
}
