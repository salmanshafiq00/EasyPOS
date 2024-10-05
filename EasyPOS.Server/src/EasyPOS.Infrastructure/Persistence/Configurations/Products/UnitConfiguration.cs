using EasyPOS.Domain.Products;

namespace EasyPOS.Infrastructure.Persistence.Configuration;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();
   
        builder.Property(t => t.Code)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(t => t.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(t => t.Operator)
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(t => t.OperatorValue)
            .HasColumnType("decimal(10, 2)")
            .IsRequired();

        //builder.Ignore(e => e.DomainEvents);
    }
}


