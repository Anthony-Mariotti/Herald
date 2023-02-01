using Herald.Core.Domain.ValueObjects.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Modules;

public class ModuleConfiguration : IEntityTypeConfiguration<HeraldModule>
{
    public void Configure(EntityTypeBuilder<HeraldModule> builder)
    {
        builder.ToTable("GuildModules");

        builder.Property<long>("Id")
            .IsRequired();

        builder.HasKey("Id");
    }
}