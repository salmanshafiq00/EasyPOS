using EasyPOS.Domain.Trades;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Trades;

internal sealed class SalePaymentConfiguration : IEntityTypeConfiguration<SalePayment>
{
    public void Configure(EntityTypeBuilder<SalePayment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.SaleId)
               .IsRequired();

        builder.Property(x => x.PaymentDate)
               .IsRequired();

        builder.Property(x => x.ReceivedAmount)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.PayingAmount)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(x => x.ChangeAmount)
               .HasColumnType("decimal(18, 2)");

        builder.Property(x => x.PaymentType)
               .IsRequired();

        builder.Property(x => x.Note)
               .HasMaxLength(500)
               .IsRequired(false);

        builder.HasOne(x => x.Sale)
               .WithMany(x => x.SalePayments) // If Sale has no navigation property for SalePayments
               .HasForeignKey(x => x.SaleId);
    }
}
