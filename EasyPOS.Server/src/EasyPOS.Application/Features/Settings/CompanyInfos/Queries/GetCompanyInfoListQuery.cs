namespace EasyPOS.Application.Features.Settings.CompanyInfos.Queries;

[Authorize(Policy = Permissions.CompanyInfos.View)]
public record GetCompanyInfoListQuery 
     : DataGridModel, ICacheableQuery<PaginatedResponse<CompanyInfoModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.CompanyInfo}_{PageNumber}_{PageSize}";
     
}

internal sealed class GetCompanyInfoQueryHandler(ISqlConnectionFactory sqlConnection) 
     : IQueryHandler<GetCompanyInfoListQuery, PaginatedResponse<CompanyInfoModel>>
{
    public async Task<Result<PaginatedResponse<CompanyInfoModel>>> Handle(GetCompanyInfoListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(CompanyInfoModel.Id)},
                t.Name AS {nameof(CompanyInfoModel.Name)},
                t.Phone AS {nameof(CompanyInfoModel.Phone)},
                t.Mobile AS {nameof(CompanyInfoModel.Mobile)},
                t.Country AS {nameof(CompanyInfoModel.Country)},
                t.State AS {nameof(CompanyInfoModel.State)},
                t.City AS {nameof(CompanyInfoModel.City)},
                t.PostalCode AS {nameof(CompanyInfoModel.PostalCode)},
                t.Address AS {nameof(CompanyInfoModel.Address)},
                t.LogoUrl AS {nameof(CompanyInfoModel.LogoUrl)},
                t.SignatureUrl AS {nameof(CompanyInfoModel.SignatureUrl)},
                t.Website AS {nameof(CompanyInfoModel.Website)}
            FROM dbo.CompanyInfos AS t
            WHERE 1 = 1
            """;


        return await PaginatedResponse<CompanyInfoModel>
            .CreateAsync(connection, sql, request);

    }
}


