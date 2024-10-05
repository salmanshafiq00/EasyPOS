using EasyPOS.Domain.Products;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Products;

public class TaxConfiguration : IEntityTypeConfiguration<Tax>
{
    public void Configure(EntityTypeBuilder<Tax> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();


        builder.Property(t => t.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(t => t.Rate)
            .HasColumnType("decimal(10, 2)")
            .IsRequired();

        //builder.Ignore(e => e.DomainEvents);
    }
}


