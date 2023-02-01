using Herald.Core.Domain.Enums;
using Herald.Core.Domain.ValueObjects.Soundtracks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Soundtracks;

public class QueueTrackConfiguration : IEntityTypeConfiguration<QueuedTrackValue>
{
    public void Configure(EntityTypeBuilder<QueuedTrackValue> builder)
    {
        builder.ToTable("QueuedTracks");

        builder.Property<long>("Id")
            .IsRequired();

        builder.HasKey("Id");

        builder.Property(p => p.Identifier)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => TrackStatus.FromValue(v));

        builder.Property(p => p.StatusReason)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => TrackStatusReason.FromValue(v));

        builder.Property(p => p.Author)
            .IsRequired()
            .HasMaxLength(160);
        
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Duration)
            .IsRequired();

        builder.Property(p => p.Livestream)
            .IsRequired();

        builder.Property(p => p.Seekable)
            .IsRequired();
        
        builder.Property(p => p.Provider)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(p => p.NotifyChannelId)
            .IsRequired();
        
        builder.Property(p => p.RequestUserId)
            .IsRequired();
        
        builder.Property(p => p.Source)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(p => p.Encoded)
            .IsRequired();
    }
}