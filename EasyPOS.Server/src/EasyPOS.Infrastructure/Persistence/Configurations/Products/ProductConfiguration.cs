using EasyPOS.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Products;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.SKU)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(x => x.Price)
            .HasColumnType("decimal(18, 2)")
            .IsRequired(true);

        builder.Property(x => x.CostPrice)
            .HasColumnType("decimal(18, 2)")
            .IsRequired(false);

        builder.Property(x => x.WholesalePrice)
            .HasColumnType("decimal(18, 2)")
            .IsRequired(false);
    }
}
