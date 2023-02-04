using Herald.Core.Domain.Entities.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Modules;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("Modules");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Released)
            .HasDefaultValue(false)
            .IsRequired();
    }
}