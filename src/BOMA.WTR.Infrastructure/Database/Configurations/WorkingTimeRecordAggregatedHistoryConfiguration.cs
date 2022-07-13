using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BOMA.WTR.Infrastructure.Database.Configurations;

public class WorkingTimeRecordAggregatedHistoryConfiguration : IEntityTypeConfiguration<WorkingTimeRecordAggregatedHistory>
{
    public void Configure(EntityTypeBuilder<WorkingTimeRecordAggregatedHistory> builder)
    {
        builder.ToTable("WorkingTimeRecordAggregatedHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.OwnsOne(x => x.SalaryInformation, salary =>
        {
            salary.Property(x => x.BaseSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.Base50PercentageSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.Base100PercentageSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.BaseSaturdaySalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.GrossBaseSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.GrossBase50PercentageSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.GrossBase100PercentageSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.GrossBaseSaturdaySalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.BonusBaseSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.BonusBase50PercentageSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.BonusBase100PercentageSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.BonusBaseSaturdaySalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.GrossSumBaseSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.GrossSumBase50PercentageSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.GrossSumBase100PercentageSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.GrossSumBaseSaturdaySalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.BonusSumSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.NightBaseSalary).HasColumnType("decimal(10,2)");
        });

        builder.Property(x => x.WorkedMinutes).HasColumnType("decimal(10,2)");
        builder.Property(x => x.WorkedHoursRounded).HasColumnType("decimal(10,2)");
        builder.Property(x => x.BaseNormativeHours).HasColumnType("decimal(10,2)");
        builder.Property(x => x.FiftyPercentageBonusHours).HasColumnType("decimal(10,2)");
        builder.Property(x => x.HundredPercentageBonusHours).HasColumnType("decimal(10,2)");
        builder.Property(x => x.SaturdayHours).HasColumnType("decimal(10,2)");
        builder.Property(x => x.NightHours).HasColumnType("decimal(10,2)");
    }
}