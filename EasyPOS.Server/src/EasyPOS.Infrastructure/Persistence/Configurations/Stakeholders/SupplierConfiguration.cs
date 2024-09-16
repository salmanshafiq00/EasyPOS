using EasyPOS.Domain.Stakeholders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Products;

internal sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(p => p.Address)
            .HasMaxLength(maxLength: 500)
            .IsRequired(false);

        builder.Property(p => p.Email)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(p => p.PhoneNo)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(p => p.Mobile)
           .HasMaxLength(20)
           .IsRequired(false);

        builder.Property(p => p.City)
           .HasMaxLength(200)
           .IsRequired(false);
    }
}
