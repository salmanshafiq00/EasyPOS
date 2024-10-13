using EasyPOS.Domain.Trades;

internal sealed class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.PurchaseDate)
               .IsRequired();

        builder.Property(x => x.ReferenceNo)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.WarehouseId)
               .IsRequired();

        builder.Property(x => x.SupplierId)
               .IsRequired();

        builder.Property(x => x.PurchaseStatusId)
               .IsRequired();

        builder.Property(x => x.AttachmentUrl)
               .HasMaxLength(255);

        builder.Property(x => x.SubTotal)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.TaxRate)
               .HasColumnType("decimal(4, 2)");

        builder.Property(x => x.TaxAmount)
               .HasColumnType("decimal(18, 2)");

        builder.Property(x => x.DiscountRate)
                .HasColumnType("decimal(3, 2)");

        builder.Property(x => x.DiscountAmount)
               .HasColumnType("decimal(18, 2)");

        builder.Property(x => x.ShippingCost)
               .HasColumnType("decimal(18, 2)");

        builder.Property(x => x.GrandTotal)
              .HasColumnType("decimal(18, 2)")
              .IsRequired();

        builder.Property(x => x.Note)
               .HasMaxLength(500);

        builder.HasMany(x => x.PurchaseDetails)
               .WithOne(x => x.Purchase)
               .HasForeignKey(x => x.PurchaseId);
    }
}


