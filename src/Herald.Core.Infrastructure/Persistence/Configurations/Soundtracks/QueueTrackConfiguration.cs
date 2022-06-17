using Herald.Core.Domain.ValueObjects.Soundtracks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configurations.Soundtracks;

public class QueueTrackConfiguration : IEntityTypeConfiguration<QueuedTrackValue>
{
    public void Configure(EntityTypeBuilder<QueuedTrackValue> builder)
    {
        builder.ToTable("QueuedTracks");

        builder.Property<long>("Id")
            .IsRequired()
            .UseHiLo("QueuedTracksSeq");

        builder.HasKey("Id");

        builder.Property("Id")
            .UseHiLo("QueuedTracksSeq");

        builder.Property(p => p.TrackId)
            .HasMaxLength(20);

        builder.Property(p => p.Title)
            .HasMaxLength(200);

        builder.Property(p => p.Author)
            .HasMaxLength(160);

        builder.Property(p => p.NotifyChannelId)
            .IsRequired();

        builder.Property(p => p.Paused)
            .IsRequired();

        builder.Property(p => p.Playing)
            .IsRequired();

        builder.Property(p => p.Played)
            .IsRequired();
    }
}