using EasyPOS.Domain.Products;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Products;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.SKU)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(x => x.SalePrice)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(x => x.CostPrice)
            .HasColumnType("decimal(18, 2)")
            .IsRequired(false);

        builder.Property(x => x.WholesalePrice)
            .HasColumnType("decimal(18, 2)")
            .IsRequired(false);

        builder.Property(t => t.DiscountAmount)
            .HasColumnType("decimal(12, 2)");

        builder.Property(t => t.DiscountRate)
            .HasColumnType("decimal(4, 2)");

        builder.Property(t => t.TaxRate)
           .HasColumnType("decimal(4, 2)");

    }
}
