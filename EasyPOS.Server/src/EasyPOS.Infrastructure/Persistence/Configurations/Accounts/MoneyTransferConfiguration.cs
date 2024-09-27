using EasyPOS.Domain.Accounts;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Accounts;

public class MoneyTransferConfiguration : IEntityTypeConfiguration<MoneyTransfer>
{
    public void Configure(EntityTypeBuilder<MoneyTransfer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();

        //builder.Ignore(e => e.DomainEvents);
    }
}


