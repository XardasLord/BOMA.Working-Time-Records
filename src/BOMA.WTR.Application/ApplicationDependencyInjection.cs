using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BOMA.WRT.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services.AddMediatR(Assembly.GetExecutingAssembly());
}