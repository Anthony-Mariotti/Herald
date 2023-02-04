using Herald.Core.Domain.Entities.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Members;
public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.GuildId)
            .IsRequired();

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.MemberId)
            .IsRequired();

        builder.Property(x => x.Balance)
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.MemberId);
    }
}
