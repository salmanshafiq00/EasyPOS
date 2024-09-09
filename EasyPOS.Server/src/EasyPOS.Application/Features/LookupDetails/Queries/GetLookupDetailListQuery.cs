using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Common.DapperQueries;
using EasyPOS.Application.Common.Security;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.LookupDetails.Queries;

[Authorize(Policy = Permissions.CommonSetup.LookupDetails.View)]
public record GetLookupDetailListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<LookupDetailModel>>
{
    [JsonInclude]
    public string CacheKey => $"LookupDetail_{PageNumber}_{PageSize}";
}

internal sealed class GetLookupDetailListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetLookupDetailListQuery, PaginatedResponse<LookupDetailModel>>
{
    public async Task<Result<PaginatedResponse<LookupDetailModel>>> Handle(GetLookupDetailListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT 
                ld.Id  AS {nameof(LookupDetailModel.Id)}, 
                ld.Name AS {nameof(LookupDetailModel.Name)}, 
                ld.Code AS {nameof(LookupDetailModel.Code)}, 
                ld.ParentId AS {nameof(LookupDetailModel.ParentId)}, 
                p.Name AS {nameof(LookupDetailModel.ParentName)}, 
                ld.Description AS {nameof(LookupDetailModel.Description)},
                IIF(ld.Status = 1, 'Active', 'Inactive') AS {nameof(LookupDetailModel.StatusName)},
                ld.LookupId AS {nameof(LookupDetailModel.LookupId)}, 
                l.Name AS {nameof(LookupDetailModel.LookupName)}
            FROM dbo.LookupDetails AS ld
            INNER JOIN dbo.Lookups l ON l.Id = ld.LookupId
            LEFT JOIN dbo.LookupDetails AS p ON p.Id = ld.ParentId
            """;

        var sqlWithOrders = $"""
            {sql} 
            ORDER BY ld.Created
            """;

        return await PaginatedResponse<LookupDetailModel>
            //.CreateAsync(connection, sql, sqlWithOrders, request.PageNumber, request.PageSize);
            .CreateAsync(connection, sql, request);
    }
}
