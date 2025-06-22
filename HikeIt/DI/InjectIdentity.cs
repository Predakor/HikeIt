using Domain.Entiites.Users;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Api.DI;

public static partial class DIextentions {
    public static WebApplicationBuilder InjectIdentity(this WebApplicationBuilder builder) {
        builder
            .Services.AddIdentity<User, IdentityRole<Guid>>(options => {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<TripDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options => {
            options.LoginPath = "/auth/login";
            options.Cookie.Name = "HikeItAuth";
            options.ExpireTimeSpan = TimeSpan.FromDays(1);
        });

        builder.Services.AddAuthorization();

        return builder;
    }

}
