using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

public class TripDbContextFactory : IDesignTimeDbContextFactory<TripDbContext> {
    public TripDbContext CreateDbContext(string[] args) {
        var optionsBuilder = new DbContextOptionsBuilder<TripDbContext>();
        string connectionString =
            "Server=localhost\\sqlexpress;Database=hikeit;Trusted_Connection=True;TrustServerCertificate=True;";
        // Provide your connection string here (or load from config/env)
        optionsBuilder.UseSqlServer(connectionString);

        return new TripDbContext(optionsBuilder.Options);
    }
}
