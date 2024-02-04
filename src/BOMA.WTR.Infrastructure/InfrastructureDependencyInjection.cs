using System.Reflection;
using AutoMapper;
using BOMA.WTR.Application.AutoMapper;
using BOMA.WTR.Application.Hangfire;
using BOMA.WTR.Application.RogerFiles;
using BOMA.WTR.Application.Salary;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.SharedKernel;
using BOMA.WTR.Infrastructure.Database;
using BOMA.WTR.Infrastructure.Database.Repositories;
using BOMA.WTR.Infrastructure.DomainService;
using BOMA.WTR.Infrastructure.ErrorHandling;
using BOMA.WTR.Infrastructure.ErrorHandling.Exceptions;
using BOMA.WTR.Infrastructure.Identity;
using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BOMA.WTR.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
        
        services.Configure<RogerFileConfiguration>(configuration.GetSection(RogerFileConfiguration.Position));
        services.Configure<SalaryConfiguration>(configuration.GetSection(SalaryConfiguration.Position));
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHealthChecks();

        services.AddDbContext<BomaDbContext>(opt => 
            opt.UseSqlServer(configuration.GetConnectionString("Boma"), 
                x => x.EnableRetryOnFailure()));

        services.AddBomaIdentity(configuration);
        
        services.AddTransient(typeof(IAggregateReadRepository<>), typeof(AggregateReadRepository<>));
        services.AddTransient(typeof(IAggregateRepository<>), typeof(AggregateRepository<>));
        
        services.AddTransient<IEmployeeWorkingTimeRecordCalculationDomainService, EmployeeWorkingTimeRecordCalculationDomainService>();
        services.AddTransient<ISalaryCalculationDomainService, SalaryCalculationDomainServiceHandler>();

        // services.AddGraphQL();
        
        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseConsole()
                .UseSqlServerStorage(configuration.GetConnectionString("Hangfire"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });
        });
        services.AddHangfireServer();
        
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddSingleton(provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile(provider.CreateScope().ServiceProvider.GetService<IEmployeeWorkingTimeRecordCalculationDomainService>() 
                                                 ?? throw new ConfigurationException($"Cannot get {nameof(IEmployeeWorkingTimeRecordCalculationDomainService)} implementation for AutoMapper configuration")));
        }).CreateMapper());

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, WebApplication webApp)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseRouting();
        
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        
        app.UseHttpsRedirection();

        app.UseBomaIdentity(webApp);

        app.UseHealthChecks("/healthz");

        // app.UseGraphQL(configuration.GetSection("GraphQL"), env);

        app.UseHangfireDashboard();
        
        RecurringJob.AddOrUpdate<ParseWorkingTimeRecordsFileJob>(
            nameof(ParseWorkingTimeRecordsFileJob),
            job => job.Execute(null, CancellationToken.None),
            configuration["Hangfire:ParseWorkingTimeRecordsFileJobCron"]);
        
        RecurringJob.AddOrUpdate<AggregateWorkingTimeRecordHistoriesJob>(
            nameof(AggregateWorkingTimeRecordHistoriesJob),
            job => job.Execute(null, CancellationToken.None),
            configuration["Hangfire:AggregateWorkingTimeRecordHistoriesJobCron"]);

        webApp.MapControllers();

        return app;
    }
}