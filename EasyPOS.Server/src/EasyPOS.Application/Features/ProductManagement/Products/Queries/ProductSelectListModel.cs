namespace EasyPOS.Application.Features.ProductManagement.Queries;

public record ProductSelectListModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal Price { get; set; }
    public Guid? PurchaseUnit { get; set; }
}
