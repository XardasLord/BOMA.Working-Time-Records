using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BOMA.WTR.Infrastructure.Identity;

public class BomaIdentityDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BomaIdentityDbContext(DbContextOptions<BomaIdentityDbContext> options, IWebHostEnvironment webHostEnvironment) 
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
        
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Identity"));
    }
}