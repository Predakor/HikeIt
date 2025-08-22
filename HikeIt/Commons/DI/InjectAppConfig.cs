using Application.Commons.Options;

namespace Api.DI;

internal static partial class DIextentions {
    public static void InjectAppConfig(this WebApplicationBuilder builder) {
        builder
            .Configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables();

        builder.Services.Configure<SeedingOptions>(
            builder.Configuration.GetSection(SeedingOptions.Seeding)
        );

        builder.Services.Configure<StorageOptions>(
            builder.Configuration.GetSection(StorageOptions.Path)
        );

        if (builder.Environment.IsDevelopment()) {
            builder.Configuration.AddUserSecrets<Program>();
        }
    }

}
