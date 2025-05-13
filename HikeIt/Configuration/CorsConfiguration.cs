namespace HikeIt.Api.Configuration;

public class CorsConfiguration(string prodPolicy, string devPolicy) {

    readonly string prodCorsPolicy = prodPolicy;
    readonly string devCorsPolicy = devPolicy;

    public WebApplicationBuilder Configure(WebApplicationBuilder builder, string[] origins) {

        builder.Services.AddCors(options => {
            options.AddPolicy(prodCorsPolicy, policy => {
                policy.WithOrigins(origins)
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
            options.AddPolicy("DevCors", policy => {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return builder;
    }




}
