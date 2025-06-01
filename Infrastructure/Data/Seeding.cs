namespace Infrastructure.Data;

using Domain.Peaks;
using Domain.Regions;
using Domain.Trips;
using Domain.Users;

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
            Id = 1,
            Name = "Janusz",
            Email = "mistrzbiznesu@wp.pl",
            BirthDay = new DateOnly(2002, 4, 15),
        },
        new()
        {
            Id = 2,
            Name = "Kasia",
            Email = "kasia.wandziak@wp.pl",
            BirthDay = new DateOnly(1995, 8, 20),
        },
        new()
        {
            Id = 3,
            Name = "Marek",
            Email = "marek.kowalski@gmail.com",
            BirthDay = new DateOnly(1988, 3, 2),
        },
        new()
        {
            Id = 4,
            Name = "Ewa",
            Email = "ewa.nowak@outlook.com",
            BirthDay = new DateOnly(1990, 12, 11),
        },
    ];

    public static readonly Trip[] Trips =
    [
        new()
        {
            Id = 1,
            TripDay = new DateOnly(2020, 12, 1),
            Duration = 8,
            Height = 1000,
            Distance = 23.7f,
            RegionId = 1,
            UserId = 1,
        },
        new()
        {
            Id = 2,
            TripDay = new DateOnly(2023, 4, 7),
            Duration = 4,
            Height = 620,
            Distance = 14.2f,
            RegionId = 22,
            UserId = 2,
        },
    ];
}
