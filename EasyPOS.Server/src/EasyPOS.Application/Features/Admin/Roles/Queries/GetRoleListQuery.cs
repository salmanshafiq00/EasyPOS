using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Common.DapperQueries;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.Roles.Queries;

public record GetRoleListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<RoleModel>>
{
    [JsonIgnore]
    public string CacheKey => $"Role__{Offset}_{PageSize}";
}

internal sealed class GetRoleListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetRoleListQuery, PaginatedResponse<RoleModel>>
{
    public async Task<Result<PaginatedResponse<RoleModel>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT 
                R.Id AS {nameof(RoleModel.Id)}, 
                R.Name AS {nameof(RoleModel.Name)} 
            FROM [identity].Roles AS R
            """;

        return await PaginatedResponse<RoleModel>
            .CreateAsync(connection, sql, request);

    }
}
