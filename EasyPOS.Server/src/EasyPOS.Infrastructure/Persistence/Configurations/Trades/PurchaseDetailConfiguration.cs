using EasyPOS.Domain.Trades;

internal sealed class PurchaseDetailConfiguration : IEntityTypeConfiguration<PurchaseDetail>
{
    public void Configure(EntityTypeBuilder<PurchaseDetail> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.PurchaseId)
               .IsRequired();

        builder.Property(x => x.ProductId)
              .IsRequired();

        builder.Property(x => x.Quantity)
               .IsRequired();

        builder.Property(x => x.BatchNo)
               .HasMaxLength(50)
               .IsRequired(false);

        builder.Property(x => x.ProductCode)
               .HasMaxLength(50)
               .IsRequired(false);

        builder.Property(x => x.ProductName)
               .HasMaxLength(250)
               .IsRequired(false);

        builder.Property(x => x.ProductUnitCost)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.ProductUnitPrice)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.ProductUnitDiscount)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.ProductUnit)
               .HasColumnType("decimal(10, 2)")
               .IsRequired();

        builder.Property(x => x.ExpiredDate);

        builder.Property(x => x.NetUnitPrice)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.DiscountAmount)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.TaxRate)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.TaxAmount)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.TotalPrice)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();
    }
}
