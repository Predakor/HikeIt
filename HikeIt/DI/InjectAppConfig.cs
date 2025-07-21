namespace Api.DI;

internal static partial class DIextentions {
    public static void InjectAppConfig(this WebApplicationBuilder builder) {
        builder
            .Configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables();

        if (builder.Environment.IsDevelopment()) {
            builder.Configuration.AddUserSecrets<Program>();
        }
    }
}
