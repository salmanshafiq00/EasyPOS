﻿using EasyPOS.Domain.Trades;

internal sealed class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.SaleDate)
               .IsRequired();

        builder.Property(x => x.ReferenceNo)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.WarehouseId)
               .IsRequired();

        builder.Property(x => x.CustomerId)
               .IsRequired();

        builder.Property(x => x.SaleStatusId)
               .IsRequired();

        builder.Property(x => x.AttachmentUrl)
               .HasMaxLength(255);

        builder.Property(x => x.OrderTax)
               .HasColumnType("decimal(18, 2)");

        builder.Property(x => x.Discount)
               .HasColumnType("decimal(18, 2)");

        builder.Property(x => x.ShippingCost)
               .HasColumnType("decimal(18, 2)");

        builder.Property(x => x.SaleNote)
               .HasMaxLength(500);

        builder.HasMany(x => x.SaleDetails)
               .WithOne(x => x.Sale)
               .HasForeignKey(x => x.SaleId);
    }
}


