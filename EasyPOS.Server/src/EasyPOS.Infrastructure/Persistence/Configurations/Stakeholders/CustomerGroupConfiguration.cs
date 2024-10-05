using EasyPOS.Domain.Stakeholders;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Stakeholders;

internal sealed class CustomerGroupConfiguration : IEntityTypeConfiguration<CustomerGroup>
{
    public void Configure(EntityTypeBuilder<CustomerGroup> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(t => t.Rate)
            .HasColumnType("decimal(10, 2)");
    }
}
