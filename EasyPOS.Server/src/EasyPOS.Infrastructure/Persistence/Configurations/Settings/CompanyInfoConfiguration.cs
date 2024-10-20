using EasyPOS.Domain.Settings;

namespace EasyPOS.Infrastructure.Persistence.Configurations.Settings;

public class CompanyInfoConfiguration : IEntityTypeConfiguration<CompanyInfo>
{
    public void Configure(EntityTypeBuilder<CompanyInfo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(t => t.Phone)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(t => t.Mobile)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(t => t.Email)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(t => t.Country)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.State)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(t => t.City)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(t => t.PostalCode)
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(t => t.Address)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(t => t.LogoUrl)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(t => t.SignatureUrl)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(t => t.Website)
            .HasMaxLength(100)
            .IsRequired(false);

        //builder.Ignore(e => e.DomainEvents);
    }
}


