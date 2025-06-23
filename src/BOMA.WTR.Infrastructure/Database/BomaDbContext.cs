using System.Reflection;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Setting;
using BOMA.WTR.Infrastructure.Database.SeedData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BOMA.WTR.Infrastructure.Database;

public class BomaDbContext(DbContextOptions<BomaDbContext> options, IWebHostEnvironment webHostEnvironment)
    : DbContext(options)
{
    public virtual DbSet<WorkingTimeRecord> WorkingTimeRecords => Set<WorkingTimeRecord>();
    public virtual DbSet<Employee> Employees => Set<Employee>();
    public virtual DbSet<Setting> Settings => Set<Setting>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{webHostEnvironment.EnvironmentName}.json")
            .AddEnvironmentVariables()
            .Build();
        
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Boma"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        var seedInitialData = new SeedInitialData();
        seedInitialData.Seed(modelBuilder);
    }
}