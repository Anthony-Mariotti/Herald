﻿using Herald.Core.Domain.Entities.Soundtracks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Soundtracks;

public class QueueConfiguration : IEntityTypeConfiguration<QueueEntity>
{
    public void Configure(EntityTypeBuilder<QueueEntity> builder)
    {
        builder.ToTable("Queues");
        builder.HasKey(p => p.Id);

        builder.Property("GuildId")
            .IsRequired();
        
        builder.HasMany(x => x.Tracks)
            .WithOne()
            .HasForeignKey("QueueId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.DomainEvents);
    }
}