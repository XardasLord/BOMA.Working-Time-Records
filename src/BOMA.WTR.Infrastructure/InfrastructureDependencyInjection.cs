using BOMA.WRT.Application.Hangfire;
using BOMA.WRT.Application.RogerFiles;
using BOMA.WRT.Infrastructure.Database;
using BOMA.WRT.Infrastructure.Database.Repositories;
using BOMA.WTR.Domain.Entities.Interfaces;
using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BOMA.WRT.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RogerFileConfiguration>(configuration.GetSection(RogerFileConfiguration.Position));
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<BomaDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("Boma")));
        services.AddTransient<IWorkingTimeRecordRepository, WorkingTimeRecordRepository>();
        
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

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseHangfireDashboard();
        RecurringJob.AddOrUpdate<ParseWorkingTimeRecordsFileJob>(
            nameof(ParseWorkingTimeRecordsFileJob),
            job => job.Execute(null, CancellationToken.None),
            configuration["Hangfire:ParseWorkingTimeRecordsFileJobCron"]);

        return app;
    }
}