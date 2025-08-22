using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Api.DI;

internal static partial class DIextentions {
    public static IServiceCollection InjectSwagger(this IServiceCollection services) {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options => {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo {
                    Title = "HikeIT Api",
                    Version = "v1",
                    Description = "HikeIt Api documentation",
                }
            );
            options.AddSecurityDefinition(
                "ApiKeyAuth",
                new OpenApiSecurityScheme {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                }
            );
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });

        return services;
    }
}
