namespace Infrastructure.Data;

using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;
using Domain.Entiites.Users;
using Domain.Trips;

internal static class DataSeed {
    public static readonly Region[] Regions =
    [
        new() { Id = 1, Name = "Tatry" },
        new() { Id = 2, Name = "Pieniny" },
        new() { Id = 3, Name = "Beskid Śląski" },
        new() { Id = 4, Name = "Beskid Żywiecki" },
        new() { Id = 5, Name = "Beskid Mały" },
        new() { Id = 6, Name = "Beskid Makowski" },
        new() { Id = 7, Name = "Beskid Wyspowy" },
        new() { Id = 8, Name = "Gorce" },
        new() { Id = 9, Name = "Beskid Sądecki" },
        new() { Id = 10, Name = "Beskid Niski" },
        new() { Id = 11, Name = "Bieszczady" },
        new() { Id = 12, Name = "Góry Świętokrzyskie" },
        new() { Id = 13, Name = "Góry Sowie" },
        new() { Id = 14, Name = "Góry Stołowe" },
        new() { Id = 15, Name = "Góry Bystrzyckie" },
        new() { Id = 16, Name = "Góry Orlickie" },
        new() { Id = 17, Name = "Góry Bialskie" },
        new() { Id = 18, Name = "Góry Złote" },
        new() { Id = 19, Name = "Góry Opawskie" },
        new() { Id = 20, Name = "Góry Bardzkie" },
        new() { Id = 21, Name = "Masyw Śnieżnika" },
        new() { Id = 22, Name = "Karkonosze" },
        new() { Id = 23, Name = "Góry Izerskie" },
        new() { Id = 24, Name = "Rudawy Janowickie" },
        new() { Id = 25, Name = "Sudety Wałbrzyskie" },
    ];

    public static readonly Peak[] Peaks =
    [
        new()
        {
            Id = 1,
            Height = 1603,
            Name = "Śnieżka",
            RegionID = 22,
        },
        new()
        {
            Id = 2,
            Height = 1346,
            Name = "Rysy",
            RegionID = 1,
        },
        new()
        {
            Id = 3,
            Height = 2050,
            Name = "Giewont",
            RegionID = 1,
        },
        new()
        {
            Id = 4,
            Height = 1367,
            Name = "Czupel",
            RegionID = 3,
        },
    ];

    public static readonly User[] Users =
[
    new()
    {
        Id = Guid.Parse("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380"),
        Name = "Janusz",
        Email = "mistrzbiznesu@wp.pl",
        BirthDay = new DateOnly(2002, 4, 15),
    },
    new()
    {
        Id = Guid.Parse("183a96d7-9c20-4b18-b65b-d5d6676b57aa"),
        Name = "Kasia",
        Email = "kasia.wandziak@wp.pl",
        BirthDay = new DateOnly(1995, 8, 20),
    },
    new()
    {
        Id = Guid.Parse("e5be7d3d-8320-4ef9-b60d-92b5464f2f1b"),
        Name = "Marek",
        Email = "marek.kowalski@gmail.com",
        BirthDay = new DateOnly(1988, 3, 2),
    },
    new()
    {
        Id = Guid.Parse("b91a0ed5-40a1-447e-8f48-c8d1e89c7c90"),
        Name = "Ewa",
        Email = "ewa.nowak@outlook.com",
        BirthDay = new DateOnly(1990, 12, 11),
    },
];


    public static readonly Trip[] Trips =
    [
    new()
    {
        Id = Guid.Parse("aa73d9ee-1b3d-4f0a-880d-6b2a4ea1d4e1"),
        TripDay = new DateOnly(2020, 12, 1),
        Duration = 8,
        Height = 1000,
        Distance = 23.7f,
        RegionId = 1,
        UserId = Guid.Parse("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380"), // Janusz
    },
    new()
    {
        Id = Guid.Parse("bfd29135-2469-4341-859f-41e42d59e0a3"),
        TripDay = new DateOnly(2023, 4, 7),
        Duration = 4,
        Height = 620,
        Distance = 14.2f,
        RegionId = 22,
        UserId = Guid.Parse("183a96d7-9c20-4b18-b65b-d5d6676b57aa"), // Kasia
    },
    ];
}
