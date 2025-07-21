using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

public class TripDbContextFactory : IDesignTimeDbContextFactory<TripDbContext> {
    public TripDbContext CreateDbContext(string[] args) {
        var optionsBuilder = new DbContextOptionsBuilder<TripDbContext>();

        // Read an environment variable to decide which DB to use
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        string connectionString;
        if (environment == "Development") {
            connectionString = "Server=localhost\\sqlexpress;Database=hikeit;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString, x => x.UseNetTopologySuite());
        }
        else {
            connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");

            optionsBuilder.UseNpgsql(connectionString, x => x.UseNetTopologySuite());
        }

        return new TripDbContext(optionsBuilder.Options);
    }
}
