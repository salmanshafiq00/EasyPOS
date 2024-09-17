namespace EasyPOS.Application.Features.Customers.Queries;

public record CustomerModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string PhoneNo { get; set; }
    public string? Mobile { get; set; }
    public Guid? CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public decimal? PreviousDue { get; set; }
    public bool IsActive { get; set; }
    public string Active { get; set; }
    public Dictionary<string, object> OptionsDataSources { get; set; } = [];

}
