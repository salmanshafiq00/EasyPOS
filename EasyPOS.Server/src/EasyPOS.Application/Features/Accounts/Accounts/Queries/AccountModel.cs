namespace EasyPOS.Application.Features.Accounts.Accounts.Queries;

public record AccountModel
{
    public Guid Id { get; set; }
        public int AccountNo {get;set;} 
    public string Name {get;set;} = string.Empty; 
    public decimal Balance {get;set;} 
    public string? Note {get;set;} 


    public Dictionary<string, object> OptionsDataSources { get; set; } = [];
}

