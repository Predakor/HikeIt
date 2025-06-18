namespace Infrastructure.Data.Seeding;

using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;
using NetTopologySuite.Geometries;

internal static partial class DataSeed {
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
    const int srid = 4326;
    public static readonly Peak[] Peaks =
    [
        new()
        {
            Id = 1,
            Height = 1603,
            Name = "Śnieżka",
            RegionID = 22, // Karkonosze
            Location = new Point(50.736, 15.739) { SRID = srid},
        },
        new()
        {
            Id = 2,
            Height = 1346,
            Name = "Rysy",
            RegionID = 1, // Tatry
            Location = new Point(49.179, 20.088) { SRID = srid},
        },
        new()
        {
            Id = 3,
            Height = 2050,
            Name = "Giewont",
            RegionID = 1, // Tatry
            Location = new Point(49.303, 19.926) { SRID = srid},
        },
        new()
        {
            Id = 4,
            Height = 1367,
            Name = "Czupel",
            RegionID = 3, // Beskid Śląski
            Location = new Point(49.784, 19.102) { SRID = srid},
        },
        new()
        {
            Id = 5,
            Height = 1725,
            Name = "Lodowy Szczyt",
            RegionID = 1, // Tatry
            Location = new Point(49.210, 20.040) { SRID = srid},
        },
        new()
        {
            Id = 6,
            Height = 1894,
            Name = "Babia Góra",
            RegionID = 4, // Beskid Żywiecki
            Location = new Point(49.606, 19.546) { SRID = srid},
        },
        new()
        {
            Id = 7,
            Height = 1153,
            Name = "Pilsko",
            RegionID = 4, // Beskid Żywiecki
            Location = new Point(49.624, 19.523) { SRID = srid},
        },
        new()
        {
            Id = 8,
            Height = 982,
            Name = "Radziejowa",
            RegionID = 9, // Beskid Sądecki
            Location = new Point(49.467, 20.535) { SRID = srid},
        },
        new()
        {
            Id = 9,
            Height = 1050,
            Name = "Ślęża",
            RegionID = 22, // Karkonosze (or Góry Świętokrzyskie - RegionID 12 might be better)
            Location = new Point(50.828, 16.700) { SRID = srid},
        },
        new()
        {
            Id = 10,
            Height = 948,
            Name = "Jaworzyna Krynicka",
            RegionID = 9, // Beskid Sądecki
            Location = new Point(49.464, 20.966) { SRID = srid},
        },
        new()
        {
            Id = 11,
            Height = 1335,
            Name = "Wielka Racza",
            RegionID = 5, // Beskid Mały
            Location = new Point(49.616, 19.269) { SRID = srid},
        },
        new()
        {
            Id = 12,
            Height = 1257,
            Name = "Luboń Wielki",
            RegionID = 6, // Beskid Makowski
            Location = new Point(49.645, 19.866) { SRID = srid},
        },
        new()
        {
            Id = 13,
            Height = 1152,
            Name = "Kiczera",
            RegionID = 3, // Beskid Śląski
            Location = new Point(49.635, 19.335) { SRID = srid},
        },
        new()
        {
            Id = 14,
            Height = 1050,
            Name = "Łysica",
            RegionID = 12, // Góry Świętokrzyskie
            Location = new Point(50.790, 20.792) { SRID = srid},
        },
        new()
        {
            Id = 15,
            Height = 944,
            Name = "Chełmiec",
            RegionID = 21, // Masyw Śnieżnika
            Location = new Point(50.716, 16.270) { SRID = srid},
        },
        new()
        {
            Id = 16,
            Height = 746,
            Name = "Trójgarb",
            RegionID = 24, // Rudawy Janowickie
            Location = new Point(51.116, 15.776) { SRID = srid},
        },
        new()
        {
            Id = 17,
            Height = 860,
            Name = "Lubomir",
            RegionID = 7, // Beskid Wyspowy
            Location = new Point(49.796, 19.847) { SRID = srid},
        },
        new()
        {
            Id = 18,
            Height = 1150,
            Name = "Mogielica",
            RegionID = 7, // Beskid Wyspowy
            Location = new Point(49.643, 20.281) { SRID = srid},
        },
        new()
        {
            Id = 19,
            Height = 970,
            Name = "Jelenia Góra",
            RegionID = 22, // Karkonosze
            Location = new Point(50.900, 15.730) { SRID = srid},
        },
        new()
        {
            Id = 20,
            Height = 1685,
            Name = "Orla Perć",
            RegionID = 1, // Tatry
            Location = new Point(49.218, 20.020) { SRID = srid},
        },
    ];
}
