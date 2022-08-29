using System.Reflection;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Infrastructure.Database.SeedData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BOMA.WTR.Infrastructure.Database;

public class BomaDbContext : DbContext
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BomaDbContext(DbContextOptions<BomaDbContext> options, IWebHostEnvironment webHostEnvironment) 
        : base(options)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{_webHostEnvironment.EnvironmentName}.json")
            .AddEnvironmentVariables()
            .Build();
        
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Boma"));
    }

    public virtual DbSet<WorkingTimeRecord> WorkingTimeRecords => Set<WorkingTimeRecord>();
    public virtual DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        var seedInitialData = new SeedInitialData();
        seedInitialData.Seed(modelBuilder);
    }
}