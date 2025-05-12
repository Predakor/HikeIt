using HikeIt.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HikeIt.Api.Data;

public class TripDbContext(DbContextOptions<TripDbContext> options) : DbContext(options)
{
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Region> Regions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Region>()
            .HasData(
                new Region { Id = 1, Name = "Tatry" },
                new Region { Id = 2, Name = "Pieniny" },
                new Region { Id = 3, Name = "Beskid Śląski" },
                new Region { Id = 4, Name = "Beskid Żywiecki" },
                new Region { Id = 5, Name = "Beskid Mały" },
                new Region { Id = 6, Name = "Beskid Makowski" },
                new Region { Id = 7, Name = "Beskid Wyspowy" },
                new Region { Id = 8, Name = "Gorce" },
                new Region { Id = 9, Name = "Beskid Sądecki" },
                new Region { Id = 10, Name = "Beskid Niski" },
                new Region { Id = 11, Name = "Bieszczady" },
                new Region { Id = 12, Name = "Góry Świętokrzyskie" },
                new Region { Id = 13, Name = "Góry Sowie" },
                new Region { Id = 14, Name = "Góry Stołowe" },
                new Region { Id = 15, Name = "Góry Bystrzyckie" },
                new Region { Id = 16, Name = "Góry Orlickie" },
                new Region { Id = 17, Name = "Góry Bialskie" },
                new Region { Id = 18, Name = "Góry Złote" },
                new Region { Id = 19, Name = "Góry Opawskie" },
                new Region { Id = 20, Name = "Góry Bardzkie" },
                new Region { Id = 21, Name = "Masyw Śnieżnika" },
                new Region { Id = 22, Name = "Karkonosze" },
                new Region { Id = 23, Name = "Góry Izerskie" },
                new Region { Id = 24, Name = "Rudawy Janowickie" },
                new Region { Id = 25, Name = "Sudety Wałbrzyskie" }
            );
        modelBuilder
            .Entity<Trip>()
            .HasData(
                new Trip
                {
                    Id = new Guid("c7e2f35f-e7ee-487d-bbc7-2e6fbb7c8c8a"),
                    TripDay = new DateOnly(2020, 12, 1),
                    Duration = 8,
                    Height = 1000,
                    Length = 23.7f,
                    RegionID = 1,
                },
                new Trip
                {
                    Id = new Guid("c7e2f35f-e7ee-487d-bbc7-2e6fbb7c8c8b"),
                    TripDay = new DateOnly(2023, 4, 7),
                    Duration = 4,
                    Height = 620,
                    Length = 14.2f,
                    RegionID = 22,
                }
            );
    }
}
