using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BOMA.WTR.Infrastructure.Database.Configurations;

public class WorkingTimeRecordConfiguration : IEntityTypeConfiguration<WorkingTimeRecord>
{
    public void Configure(EntityTypeBuilder<WorkingTimeRecord> builder)
    {
        builder.ToTable("WorkingTimeRecords");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.EventType)
            .HasColumnName("EventType")
            .HasConversion(new EnumToNumberConverter<RecordEventType, byte>())
            .IsRequired();

        builder
            .Property(x => x.OccuredAt)
            .HasColumnName("OccuredAt")
            .IsRequired();

        builder.Property(x => x.DepartmentId)
            .HasColumnName("DepartmentId")
            .IsRequired();
        
        builder.HasOne(x => x.Department)
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}