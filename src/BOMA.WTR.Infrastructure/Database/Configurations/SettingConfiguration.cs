using BOMA.WTR.Domain.AggregateModels.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BOMA.WTR.Infrastructure.Database.Configurations;

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("Settings");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.Key).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Value).IsRequired();
        builder.Property(x => x.Type).HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(255);
        
        builder.HasIndex(x => x.Key).IsUnique();
    }
}