using EasyPOS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyPOS.Infrastructure.Persistence.Configurations;

internal sealed class LookupConfiguration : IEntityTypeConfiguration<Lookup>
{
    public void Configure(EntityTypeBuilder<Lookup> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(t => t.Name)
            .IsUnique();

        builder.HasIndex(t => t.ParentId);

        builder.Property(t => t.Code)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(500);
    }
}
