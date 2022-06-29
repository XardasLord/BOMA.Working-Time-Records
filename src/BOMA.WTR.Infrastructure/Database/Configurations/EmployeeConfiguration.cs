using BOMA.WTR.Domain.AggregateModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOMA.WRT.Infrastructure.Database.Configurations;

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

        builder.Property(x => x.RcpId)
            .HasColumnName("RcpId")
            .IsRequired();

        builder.HasMany(x => x.WorkingTimeRecords)
            .WithOne()
            .IsRequired()
            .HasForeignKey("UserId");
        
        builder.Metadata
            .FindNavigation(nameof(Employee.WorkingTimeRecords))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}