﻿using EasyPOS.Domain.Accounts;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Accounts;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();

        builder.Property(t => t.Balance)
            .HasColumnType("decimal(18, 2)");

        builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
        //builder.Ignore(e => e.DomainEvents);
    }
}


