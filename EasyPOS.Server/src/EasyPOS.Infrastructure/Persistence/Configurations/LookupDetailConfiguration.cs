using EasyPOS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyPOS.Infrastructure.Persistence.Configurations;

internal sealed class LookupDetailConfiguration : IEntityTypeConfiguration<LookupDetail>
{
    public void Configure(EntityTypeBuilder<LookupDetail> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(t => t.Name).IsUnique();

        builder.Property(t => t.Code)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.HasOne(t => t.Lookup)
            .WithMany()
            .HasForeignKey(t => t.LookupId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasIndex(t => t.LookupId);
    }
}
