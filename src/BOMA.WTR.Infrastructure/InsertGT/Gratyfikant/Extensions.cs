using BOMA.WTR.Application.InsertGT.Gratyfikant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BOMA.WTR.Infrastructure.InsertGT.Gratyfikant;

public static class Extensions
{
    private const string OptionsSectionName = "Gratyfikant";
    
    public static IServiceCollection AddGratyfikant(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GratyfikantOptions>(configuration.GetRequiredSection(OptionsSectionName));

        services.AddTransient<IGratyfikantService, GratyfikantService>();
        
        return services;
    }
}