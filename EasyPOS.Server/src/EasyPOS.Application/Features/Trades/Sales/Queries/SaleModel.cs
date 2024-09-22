using EasyPOS.Domain.Trades;

namespace EasyPOS.Application.Features.Trades.Sales.Queries;

public record SaleModel
{
    public Guid Id { get; set; }
    public DateOnly SaleDate { get; set; }
    public string ReferenceNo { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BullerId { get; set; }
    public string? AttachmentUrl { get; set; }
    public decimal? OrderTax { get; set; }
    public Guid OrderDiscountTypeId { get; set; }
    public decimal? Discount { get; set; }
    public decimal? ShippingCost { get; set; }
    public Guid SaleStatusId { get; set; }
    public Guid PaymentStatusId { get; set; }
    public string? SaleNote { get; set; }
    public string? StaffNote { get; set; }

    public List<SaleDetail> SaleDetails { get; set; } = [];
    public Dictionary<string, object> OptionsDataSources { get; set; } = [];

}

public record SaleDetailModel
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string BatchNo { get; set; }
    public DateOnly? ExpiredDate { get; set; }
    public decimal NetUnitCost { get; set; }
    public decimal Decimal { get; set; }
    public decimal Tax { get; set; }
    public decimal SubTotal { get; set; }

}
