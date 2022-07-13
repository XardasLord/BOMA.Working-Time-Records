using System.Reflection;
using BOMA.WTR.Application.Hangfire;
using BOMA.WTR.Application.RogerFiles;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Infrastructure.Database;
using BOMA.WTR.Infrastructure.Database.Repositories;
using BOMA.WTR.Infrastructure.DomainService;
using BOMA.WTR.Infrastructure.ErrorHandling;
using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using MediatR;
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
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<BomaDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("Boma")));
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        
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
        
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddTransient<IEmployeeWorkingTimeRecordCalculationDomainService, EmployeeWorkingTimeRecordCalculationDomainService>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseHangfireDashboard();
        
        RecurringJob.AddOrUpdate<ParseWorkingTimeRecordsFileJob>(
            nameof(ParseWorkingTimeRecordsFileJob),
            job => job.Execute(null, CancellationToken.None),
            configuration["Hangfire:ParseWorkingTimeRecordsFileJobCron"]);
        
        RecurringJob.AddOrUpdate<AggregateWorkingTimeRecordHistoriesJob>(
            nameof(AggregateWorkingTimeRecordHistoriesJob),
            job => job.Execute(null, CancellationToken.None),
            configuration["Hangfire:AggregateWorkingTimeRecordHistoriesJobCron"]);

        return app;
    }
}