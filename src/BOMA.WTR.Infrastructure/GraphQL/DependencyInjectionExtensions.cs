using BOMA.WTR.Infrastructure.GraphQL.Queries;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BOMA.WTR.Infrastructure.GraphQL;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddGraphQL(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .InitializeOnStartup()
            .AddQueryType<BomaGraphQLQuery>()
            .AddProjections()
            .AddFiltering()
            .AddSorting();

        return services;
    }
    
    public static IApplicationBuilder UseGraphQL(
        this IApplicationBuilder app,
        IConfiguration graphQlConfigurationSection, 
        IWebHostEnvironment env)
    {
        var graphQLEndpoint = graphQlConfigurationSection.GetSection("Endpoint").Value;

        return app.UseEndpoints(x => x.MapGraphQL(graphQLEndpoint)
            .WithOptions(new GraphQLServerOptions
            {
                Tool =
                {
                    Enable = env.IsDevelopment(),
                    Title = "BOMA ECP GraphQL"
                }
            }));
    }
}