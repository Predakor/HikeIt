using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Api.DI;

public static partial class DIextentions {
    public static WebApplicationBuilder InjectSwagger(this WebApplicationBuilder builder) {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options => {
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

        return builder;
    }
}
