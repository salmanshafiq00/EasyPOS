using EasyPOS.Domain.Trades;

internal sealed class SaleDetailConfiguration : IEntityTypeConfiguration<SaleDetail>
{
    public void Configure(EntityTypeBuilder<SaleDetail> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.SaleId)
               .IsRequired();

        builder.Property(x => x.ProductId)
               .IsRequired();

        builder.Property(x => x.Quantity)
               .IsRequired();

        builder.Property(x => x.BatchNo)
               .HasMaxLength(50)
               .IsRequired(false);

        builder.Property(x => x.ExpiredDate);

        builder.Property(x => x.NetUnitCost)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.Tax)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.SubTotal)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();
    }
}
