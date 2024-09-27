namespace EasyPOS.Application.Features.Accounts.MoneyTransfers.Queries;

public record MoneyTransferModel
{
    public Guid Id { get; set; }
    public Guid FromAccountId {get;set;} 
    public Guid ToAccountId {get;set;} 
    public decimal Amount {get;set;} 


    public Dictionary<string, object> OptionsDataSources { get; set; } = [];
}

