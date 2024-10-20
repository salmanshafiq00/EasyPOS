namespace EasyPOS.Application.Features.Settings.CompanyInfos.Queries;

public record CompanyInfoModel
{
    public Guid Id { get; set; }
    public string Name {get;set;} = string.Empty; 
    public string? Phone {get;set;} 
    public string? Mobile {get;set;}
    public string? Email { get; set; }
    public string? Country {get;set;} 
    public string? State {get;set;} 
    public string? City {get;set;} 
    public string? PostalCode {get;set;} 
    public string? Address {get;set;} 
    public string? LogoUrl {get;set;} 
    public string? SignatureUrl {get;set;} 
    public string? Website {get;set;} 


    public Dictionary<string, object> OptionsDataSources { get; set; } = [];
}

