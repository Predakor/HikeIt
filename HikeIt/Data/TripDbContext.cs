using HikeIt.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HikeIt.Api.Data;

public class TripDbContext(DbContextOptions<TripDbContext> options) : DbContext(options) {
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Peak> Peaks { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
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
                new Trip {
                    Id = 1,
                    TripDay = new DateOnly(2020, 12, 1),
                    Duration = 8,
                    Height = 1000,
                    Length = 23.7f,
                    RegionID = 1,
                },
                new Trip {
                    Id = 2,
                    TripDay = new DateOnly(2023, 4, 7),
                    Duration = 4,
                    Height = 620,
                    Length = 14.2f,
                    RegionID = 22,
                }
            );
        modelBuilder
            .Entity<Peak>()
            .HasData(
                new Peak {
                    Id = 1,
                    Height = 1603,
                    Name = "Śnieżka",
                    RegionID = 22,
                },
                new Peak {
                    Id = 2,
                    Height = 1346,
                    Name = "Rysy",
                    RegionID = 1,
                },
                new Peak {
                    Id = 3,
                    Height = 2050,
                    Name = "Giewont",
                    RegionID = 1,
                },
                new Peak {
                    Id = 4,
                    Height = 1367,
                    Name = "Czupel",
                    RegionID = 3,
                }
            );
        modelBuilder
            .Entity<User>()
            .HasData(
                new User {
                    Id = 1,
                    Name = "Janusz",
                    Email = "mistrzbiznesu@wp.pl",
                    BirthDay = new DateOnly(2002, 4, 15),
                },
                new User {
                    Id = 2,
                    Name = "Kasia",
                    Email = "kasia.wandziak@wp.pl",
                    BirthDay = new DateOnly(1995, 8, 20),
                },
                new User {
                    Id = 3,
                    Name = "Marek",
                    Email = "marek.kowalski@gmail.com",
                    BirthDay = new DateOnly(1988, 3, 2),
                },
                new User {
                    Id = 4,
                    Name = "Ewa",
                    Email = "ewa.nowak@outlook.com",
                    BirthDay = new DateOnly(1990, 12, 11),
                }
            );
    }
}
