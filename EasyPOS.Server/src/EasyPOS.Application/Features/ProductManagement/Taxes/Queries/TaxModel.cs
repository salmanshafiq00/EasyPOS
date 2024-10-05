namespace EasyPOS.Application.Features.ProductManagement.Taxes.Queries;

public record TaxModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; }


    public Dictionary<string, object> OptionsDataSources { get; set; } = [];
}

