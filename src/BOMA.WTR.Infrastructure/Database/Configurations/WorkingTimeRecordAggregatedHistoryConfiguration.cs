using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
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

        builder
            .Property(x => x.MissingRecordEventType)
            .HasColumnName("MissingRecordEventType")
            .HasConversion(new EnumToNumberConverter<MissingRecordEventType, byte>())
            .HasDefaultValue(null);

        builder.OwnsOne(x => x.SalaryInformation, salary =>
        {
            salary.Property(x => x.PercentageBonusSalary).HasColumnType("decimal(10,2)");
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
            salary.Property(x => x.NightBaseSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.HolidaySalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.SicknessSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.AdditionalSalary).HasColumnType("decimal(10,2)");
            salary.Property(x => x.MinSalaryCompensationFactor).HasColumnType("decimal(10,4)");
            salary.Property(x => x.MinSalaryCompensationAmount).HasColumnType("decimal(10,2)");
            salary.Property(x => x.FinalSumSalary).HasColumnType("decimal(10,2)");
        });

        builder.Property(x => x.WorkedMinutes).HasColumnType("decimal(10,2)");
        builder.Property(x => x.WorkedHoursRounded).HasColumnType("decimal(10,2)");
        builder.Property(x => x.BaseNormativeHours).HasColumnType("decimal(10,2)");
        builder.Property(x => x.FiftyPercentageBonusHours).HasColumnType("decimal(10,2)");
        builder.Property(x => x.HundredPercentageBonusHours).HasColumnType("decimal(10,2)");
        builder.Property(x => x.SaturdayHours).HasColumnType("decimal(10,2)");
        builder.Property(x => x.NightHours).HasColumnType("decimal(10,2)");
        
        builder.Property(x => x.DepartmentId)
            .HasColumnName("DepartmentId")
            .IsRequired();
        
        builder.Property(x => x.ShiftType)
            .HasColumnName("ShiftType")
            .HasConversion(new EnumToNumberConverter<ShiftType, byte>())
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasColumnName("IsActive")
            .IsRequired();
    }
}