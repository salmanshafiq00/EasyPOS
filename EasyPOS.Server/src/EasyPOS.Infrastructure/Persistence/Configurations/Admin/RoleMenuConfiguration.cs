using EasyPOS.Domain.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Admin;

internal sealed class RoleMenuConfiguration : IEntityTypeConfiguration<RoleMenu>
{
    public void Configure(EntityTypeBuilder<RoleMenu> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(100);

        builder.HasIndex(t => new { t.RoleId, t.AppMenuId })
            .IsUnique();

        builder.HasOne<AppMenu>()
            .WithMany()
            .HasForeignKey(x => x.AppMenuId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
