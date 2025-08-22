using Domain.Common.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Commons.Databases;

class NullDomainEventDispatcher : IEventPublisher {
    public Task PublishAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default
    ) {
        return Task.CompletedTask;
    }

    public Task PublishAsync(
        IDomainEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        return Task.CompletedTask;
    }
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TripDbContext> {
    public TripDbContext CreateDbContext(string[] args) {
        var environment =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        bool isProduction = environment == "Production";

        string connectionString;

        if (isProduction) {
            connectionString =
                Environment.GetEnvironmentVariable("ConnectionStrings__TripDbCS")
                ?? throw new Exception("Db connection string not found in enviroment");
        }
        else {
            connectionString =
                "Host=127.0.0.1;Port=54322;Database=postgres;Username=postgres;Password=postgres";
        }

        Console.WriteLine($"Enviroment:: {environment}");
        Console.WriteLine($"ConnectionString:" + connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<TripDbContext>();
        optionsBuilder.UseNpgsql(connectionString, x => x.UseNetTopologySuite());

        return new TripDbContext(optionsBuilder.Options, new NullDomainEventDispatcher());
    }
}
