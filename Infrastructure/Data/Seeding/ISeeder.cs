namespace Infrastructure.Data.Seeding;

public interface ISeeder {
    void Seed(TripDbContext dbContext);
}
