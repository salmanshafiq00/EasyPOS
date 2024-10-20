namespace EasyPOS.Application.Features.Settings.CompanyInfos.Queries;

public record GetCompanyInfoByIdQuery : ICacheableQuery<CompanyInfoModel>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.CompanyInfo}";
    [JsonIgnore]
    public TimeSpan? Expiration => null;
    public bool? AllowCache => false;
}

internal sealed class GetCompanyInfoByIdQueryHandler(ISqlConnectionFactory sqlConnection)
     : IQueryHandler<GetCompanyInfoByIdQuery, CompanyInfoModel>
{

    public async Task<Result<CompanyInfoModel>> Handle(GetCompanyInfoByIdQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT TOP 1
                t.Id AS {nameof(CompanyInfoModel.Id)},
                t.Name AS {nameof(CompanyInfoModel.Name)},
                t.Phone AS {nameof(CompanyInfoModel.Phone)},
                t.Mobile AS {nameof(CompanyInfoModel.Mobile)},
                t.Email AS {nameof(CompanyInfoModel.Email)},
                t.Country AS {nameof(CompanyInfoModel.Country)},
                t.State AS {nameof(CompanyInfoModel.State)},
                t.City AS {nameof(CompanyInfoModel.City)},
                t.PostalCode AS {nameof(CompanyInfoModel.PostalCode)},
                t.Address AS {nameof(CompanyInfoModel.Address)},
                t.LogoUrl AS {nameof(CompanyInfoModel.LogoUrl)},
                t.SignatureUrl AS {nameof(CompanyInfoModel.SignatureUrl)},
                t.Website AS {nameof(CompanyInfoModel.Website)}
            FROM dbo.CompanyInfos AS t
            """;

        var company = await connection.QueryFirstOrDefaultAsync<CompanyInfoModel>(sql);

        return company is not null 
            ? company 
            : new CompanyInfoModel();
    }
}

