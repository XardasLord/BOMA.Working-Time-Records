using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Setting;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.Database.SeedData;

public class SeedInitialData
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().HasData(Department.Create(1, "Magazyn"));
        modelBuilder.Entity<Department>().HasData(Department.Create(2, "Akcesoria"));
        modelBuilder.Entity<Department>().HasData(Department.Create(3, "Produkcja"));
        modelBuilder.Entity<Department>().HasData(Department.Create(4, "Pakowanie"));
        modelBuilder.Entity<Department>().HasData(Department.Create(5, ""));
        modelBuilder.Entity<Department>().HasData(Department.Create(6, "BOMA"));
        modelBuilder.Entity<Department>().HasData(Department.Create(7, "Zlecenia"));
        modelBuilder.Entity<Department>().HasData(Department.Create(8, "Agencja"));

        var setting = new Setting(
            "MinimumWage",
            "4666",
            "int",
            "Minimalne wynagrodzenie pracownika."
        )
        {
            Id = 1
        };

        modelBuilder.Entity<Setting>().HasData(setting);
    }
}