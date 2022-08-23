using System.Reflection;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Infrastructure.Database.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BOMA.WTR.Infrastructure.Database;

public class BomaDbContext : DbContext
{
    public BomaDbContext(DbContextOptions<BomaDbContext> options) 
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .AddJsonFile("appsettings.Production.json")
            .Build();
        
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Boma"));
    }

    public virtual DbSet<WorkingTimeRecord> WorkingTimeRecords { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        var seedInitialData = new SeedInitialData();
        seedInitialData.Seed(modelBuilder);
    }
}