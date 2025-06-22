using Domain.Entiites.Users;
using Domain.Trips;

namespace Infrastructure.Data.Seeding;

internal static partial class DataSeed {

    public static readonly User[] Users =
    [
        new()
        {
            Id = Guid.Parse("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380"),
            FirstName ="Janusz",
            LastName="Kowalski",
            UserName = "Janusz",
            Email = "mistrzbiznesu@wp.pl",
            BirthDay = new DateOnly(2002, 4, 15),
        },
        new()
        {
            Id = Guid.Parse("183a96d7-9c20-4b18-b65b-d5d6676b57aa"),
            FirstName ="Janusz",
            LastName="Kowalski",
            UserName = "Kasia",
            Email = "kasia.wandziak@wp.pl",
            BirthDay = new DateOnly(1995, 8, 20),
        },
        new()
        {
            Id = Guid.Parse("e5be7d3d-8320-4ef9-b60d-92b5464f2f1b"),
            FirstName ="Janusz",
            LastName="Kowalski",
            UserName = "Marek",
            Email = "marek.kowalski@gmail.com",
            BirthDay = new DateOnly(1988, 3, 2),
        },
        new()
        {
            Id = Guid.Parse("b91a0ed5-40a1-447e-8f48-c8d1e89c7c90"),
            FirstName ="Janusz",
            LastName="Kowalski",
            UserName = "Ewa",
            Email = "ewa.nowak@outlook.com",
            BirthDay = new DateOnly(1990, 12, 11),
        },
    ];
    public static readonly Trip[] Trips =
    [
        Trip.Create(
            "Wycieczka na śnieżke",
            new DateOnly(2023, 5, 1),
            Users[0].Id,
            Regions[21].Id,
            Guid.Parse("b91a0ed5-40a1-447e-8f48-c8d1e89c7c91")
        ),
        Trip.Create(
            "Śnieżne kotły",
            new DateOnly(2025, 1, 16),
            Users[0].Id,
            Regions[21].Id,
            Guid.Parse("b91a0ed5-40a1-447e-8f48-c8d1e89c7c92")
        ),
    ];
}