using EasyPOS.Domain.Trades;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Trades;

internal sealed class PurchasePaymentConfiguration : IEntityTypeConfiguration<PurchasePayment>
{
    public void Configure(EntityTypeBuilder<PurchasePayment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.PurchaseId)
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
               .HasMaxLength(500);

        builder.HasOne(x => x.Purchase)
               .WithMany(x => x.PurchasePayments) 
               .HasForeignKey(x => x.PurchaseId);
    }
}
