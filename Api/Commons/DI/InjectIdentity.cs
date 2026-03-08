using Domain.Users.Root;
using Infrastructure.Commons.Databases;
using Microsoft.AspNetCore.Identity;

namespace Api.DI;

internal static partial class DIextentions
{
    public static IServiceCollection InjectIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var basePath = configuration.GetSection("Cors").GetValue<string>("BasePath") ?? "/";

        services
            .AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<TripDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/auth/login";
            options.Cookie.Name = "HikeItAuth";

            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.HttpOnly = true;

            options.ExpireTimeSpan = TimeSpan.FromDays(1);

            options.Events.OnRedirectToLogin = ctx =>
            {
                if (
                    ctx.Request.Path.StartsWithSegments("/api")
                    && ctx.Response.StatusCode == StatusCodes.Status200OK
                )
                {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
                var redirect = Path.Combine(basePath, options.LoginPath);
                ctx.Response.Redirect(redirect);
                return Task.CompletedTask;
            };
        });

        services.AddAuthorization();

        return services;
    }
}
