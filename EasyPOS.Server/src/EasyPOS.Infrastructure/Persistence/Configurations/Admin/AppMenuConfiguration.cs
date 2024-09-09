using EasyPOS.Domain.Admin;
using EasyPOS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Admin;

internal sealed class AppMenuConfiguration : IEntityTypeConfiguration<AppMenu>
{
    public void Configure(EntityTypeBuilder<AppMenu> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(t => t.Label)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(t => t.Label).IsUnique(false);

        builder.Property(t => t.RouterLink)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Tooltip)
            .HasMaxLength(100);

        builder.Property(t => t.Description)
            .HasMaxLength(200);

        builder.HasOne<LookupDetail>()
            .WithMany()
            .HasForeignKey(x => x.MenuTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
