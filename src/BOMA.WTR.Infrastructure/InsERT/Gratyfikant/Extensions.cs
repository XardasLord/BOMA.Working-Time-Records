using BOMA.WTR.Application.InsERT;
using BOMA.WTR.Application.InsERT.Gratyfikant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BOMA.WTR.Infrastructure.InsERT.Gratyfikant;

public static class Extensions
{
    private const string OptionsSectionName = "Gratyfikant";
    
    public static IServiceCollection AddGratyfikant(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new GratyfikantOptions();
        configuration.Bind(OptionsSectionName, options);
        services.AddSingleton(options);

        services.AddTransient<IGratyfikantService, GratyfikantApiService>();
            
        services.AddHttpClient(ExternalHttpClientNames.GratyfikantHttpClientName, config =>
        {
            config.BaseAddress = new Uri(options.ApiUrl);
            config.Timeout = new TimeSpan(0, 2, 0);
            config.DefaultRequestHeaders.Clear();
        });
        
        return services;
    }
}