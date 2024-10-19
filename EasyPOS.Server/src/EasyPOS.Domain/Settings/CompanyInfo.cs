namespace EasyPOS.Domain.Settings;

public class CompanyInfo: BaseAuditableEntity
{
    public string Name { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string Country { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string? LogoUrl { get; set; }
    public string? SignatureUrl { get; set; }
    public string? Website { get; set; }
}
