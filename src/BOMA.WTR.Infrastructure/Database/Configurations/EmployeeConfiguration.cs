using BOMA.WTR.Domain.AggregateModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOMA.WTR.Infrastructure.Database.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.OwnsOne(x => x.Name, x =>
        {
            x.Property(p => p.FirstName).HasColumnName("FirstName").HasMaxLength(64).IsRequired();
            x.Property(p => p.LastName).HasColumnName("LastName").HasMaxLength(64).IsRequired();
        });

        builder.OwnsOne(x => x.Salary, salary =>
        {
            salary.Property(p => p.Amount)
                .HasColumnName("BaseSalaryAmount")
                .HasDefaultValue(0)
                .HasColumnType("money");
            
            salary.Property(p => p.Currency)
                .HasColumnName("Currency")
                .HasDefaultValue("PLN");
        });
        
        builder.OwnsOne(x => x.SalaryBonusPercentage, bonus =>
        {
            bonus.Property(p => p.Value)
                .HasColumnName("PercentageBonusSalary")
                .HasDefaultValue(0)
                .HasColumnType("money");
        });

        builder.Property(x => x.RcpId)
            .HasColumnName("RcpId")
            .IsRequired();

        builder.HasMany(x => x.WorkingTimeRecords)
            .WithOne()
            .IsRequired()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.WorkingTimeRecordAggregatedHistories)
            .WithOne()
            .IsRequired()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Department)
            .WithMany()
            .IsRequired()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Metadata
            .FindNavigation(nameof(Employee.WorkingTimeRecords))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Metadata
            .FindNavigation(nameof(Employee.WorkingTimeRecordAggregatedHistories))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}