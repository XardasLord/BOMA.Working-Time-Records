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
        services.AddDbContext<BomaIdentityDbContext>(opt => 
            opt.UseSqlServer(configuration.GetConnectionString("Identity"), 
                x => x.EnableRetryOnFailure()));

        services.AddAuthorization();
        
        services.AddIdentityApiEndpoints<IdentityUser>(options => 
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BomaIdentityDbContext>();

        return services;
    }

    public static IApplicationBuilder UseBomaIdentity(this IApplicationBuilder app, WebApplication webApp)
    {
        app.UseAuthorization();
        
        webApp.MapIdentityApi<IdentityUser>();

        return app;
    }
}