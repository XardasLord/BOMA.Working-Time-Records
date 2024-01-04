using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BOMA.WTR.Infrastructure.Identity;

public static class IdentityDependencyInjection
{
    public static IServiceCollection AddBomaIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication();
        services.AddAuthorization();
        
        services.AddDbContext<BomaIdentityDbContext>(opt => 
            opt.UseSqlServer(configuration.GetConnectionString("Identity"), 
                x => x.EnableRetryOnFailure()));

        services.AddIdentityApiEndpoints<IdentityUser>()
            .AddEntityFrameworkStores<BomaIdentityDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        });

        // services.AddTransient<IEmailSender, EmailSender>();
        // services.Configure<EmailServerConfiguration>(configuration.GetSection("EmailServer"));

        return services;
    }

    public static IApplicationBuilder UseBomaIdentity(this IApplicationBuilder app, WebApplication webApp)
    {
        app.UseAuthorization();
        
        webApp.MapIdentityApi<IdentityUser>();

        return app;
    }
}