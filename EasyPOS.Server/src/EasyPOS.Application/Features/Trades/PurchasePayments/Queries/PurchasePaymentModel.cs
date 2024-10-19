namespace EasyPOS.Application.Features.Trades.PurchasePayments.Queries;

public record PurchasePaymentModel
{
    public Guid Id { get; set; }
    public Guid PurchaseId {get;set;} 
    public DateTime PaymentDate {get;set;} 
    public decimal ReceivedAmount {get;set;} 
    public decimal PayingAmount {get;set;} 
    public decimal ChangeAmount {get;set;} 
    public Guid? PaymentType {get;set;} 
    public string PaymentTypeName {get;set;} 
    public string CreatedBy {get;set;} 
    public string? Note {get;set;} 
    public string? PaymentDateString { get;set;} 


    public Dictionary<string, object> OptionsDataSources { get; set; } = [];
}

