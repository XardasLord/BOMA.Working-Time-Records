using System.Reflection;
using AutoMapper;
using BOMA.WTR.Application.AutoMapper;
using BOMA.WTR.Application.MediatorRequestPipelines;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BOMA.WTR.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        services.AddSingleton(provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile(provider.CreateScope().ServiceProvider.GetService<IEmployeeWorkingTimeRecordCalculationDomainService>()));
        }).CreateMapper());
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}