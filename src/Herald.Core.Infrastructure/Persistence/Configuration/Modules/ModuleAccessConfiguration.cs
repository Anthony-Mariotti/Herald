using Herald.Core.Domain.Entities.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Modules;
public class ModuleAccessConfiguration : IEntityTypeConfiguration<ModuleAccess>
{
    public void Configure(EntityTypeBuilder<ModuleAccess> builder)
    {
        builder.ToTable("ModuleAccess");

        builder.HasKey(x => new { x.ModuleId, x.GuildId });

        builder.Property(x => x.ModuleId)
            .IsRequired();

        builder.Property(x => x.GuildId)
            .IsRequired();

        builder.Property(x => x.HasAccess)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne(x => x.Module)
            .WithMany()
            .HasForeignKey(x => x.ModuleId);
    }
}
