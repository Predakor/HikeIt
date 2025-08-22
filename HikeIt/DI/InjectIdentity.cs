using Domain.Users;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Api.DI;

internal static partial class DIextentions {
    public static IServiceCollection InjectIdentity(this IServiceCollection services) {
        services
            .AddIdentity<User, IdentityRole<Guid>>(options => {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<TripDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options => {
            options.LoginPath = "/auth/login";
            options.Cookie.Name = "HikeItAuth";

            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.HttpOnly = true;

            options.ExpireTimeSpan = TimeSpan.FromDays(1);

            options.Events.OnRedirectToLogin = ctx => {
                if (
                    ctx.Request.Path.StartsWithSegments("/api")
                    && ctx.Response.StatusCode == StatusCodes.Status200OK
                ) {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }

                ctx.Response.Redirect(ctx.RedirectUri);
                return Task.CompletedTask;
            };
        });

        services.AddAuthorization();

        return services;
    }
}
