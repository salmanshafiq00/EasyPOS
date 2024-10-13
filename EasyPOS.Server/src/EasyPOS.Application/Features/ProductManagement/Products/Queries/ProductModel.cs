using EasyPOS.Domain.Common.Enums;
using EasyPOS.Domain.Enums;

namespace EasyPOS.Application.Features.ProductManagement.Queries;

public record ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public Guid ProductTypeId { get; set; }
    public Guid? BrandId { get; set; }
    public string? Code { get; set; }
    public string? SKU { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal? WholesalePrice { get; set; }
    public Guid? Unit { get; set; }
    public Guid? SaleUnit { get; set; }
    public Guid? PurchaseUnit { get; set; }
    public int? AlertQuantity { get; set; }
    public Guid? BarCodeType { get; set; }
    public Guid? QRCodeType { get; set; }
    public TaxMethod TaxMethod { get; set; }
    public decimal TaxRate { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal Discount { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public string[] PhotoUrls { get; set; } = [];

    public Dictionary<string, object> OptionsDataSources { get; set; } = [];

}

public record ProductUpsertModel
{
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public Guid ProductTypeId { get; set; }
    public Guid? BrandId { get; set; }
    public string? Code { get; set; }
    public string? SKU { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal? WholesalePrice { get; set; }
    public Guid? Unit { get; set; }
    public Guid? SaleUnit { get; set; }
    public Guid? PurchaseUnit { get; set; }
    public int? AlertQuantity { get; set; }
    public Guid? BarCodeType { get; set; }
    public Guid? QRCodeType { get; set; }
    public TaxMethod TaxMethod { get; set; }
    public decimal TaxRate { get; set; }
    //public DiscountType? DiscountType { get; set; }
    //public decimal? DiscountRate { get; set; }
    //public decimal? DiscountAmount { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
