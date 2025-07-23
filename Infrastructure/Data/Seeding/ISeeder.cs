namespace Infrastructure.Data.Seeding;

public interface ISeeder {
    Task Seed(TripDbContext dbContext);
}
