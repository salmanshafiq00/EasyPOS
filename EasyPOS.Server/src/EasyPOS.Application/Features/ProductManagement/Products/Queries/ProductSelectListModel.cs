using EasyPOS.Domain.Enums;

namespace EasyPOS.Application.Features.ProductManagement.Queries;

public record ProductSelectListModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public Guid? PurchaseUnit { get; set; }
    public decimal TaxRate { get; set; }
    public TaxMethod TaxMethod { get; set; }
}
