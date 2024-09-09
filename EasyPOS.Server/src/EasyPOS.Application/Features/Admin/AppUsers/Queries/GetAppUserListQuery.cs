using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Common.DapperQueries;
using EasyPOS.Application.Common.Security;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.AppUsers.Queries;

[Authorize(Policy = Permissions.Admin.ApplicationUsers.View)]
public record GetAppUserListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<AppUserModel>>
{
    [JsonIgnore]
    public string CacheKey => $"AppUser__{Offset}_{PageSize}";
}

internal sealed class GetAppUserListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetAppUserListQuery, PaginatedResponse<AppUserModel>>
{
    public async Task<Result<PaginatedResponse<AppUserModel>>> Handle(GetAppUserListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        //var sql = $"""
        //    SELECT 
        //        U.Id AS {nameof(AppUserModel.Id)}, 
        //        U.FirstName AS {nameof(AppUserModel.FirstName)}, 
        //        U.LastName AS {nameof(AppUserModel.LastName)}, 
        //        U.Username AS {nameof(AppUserModel.Username)}, 
        //        U.Email AS {nameof(AppUserModel.Email)} , 
        //        U.PhoneNumber AS {nameof(AppUserModel.PhoneNumber)},
        //        IIF(U.IsActive = 1, 'Active', 'Inactive') AS {nameof(AppUserModel.Status)},
        //        U.PhotoUrl AS {nameof(AppUserModel.PhotoUrl)},
        //        (SELECT STRING_AGG(R.Name, ', ')
        //        FROM [identity].UserRoles AS UR
        //        INNER JOIN [identity].Roles AS R ON R.Id = UR.RoleId) AS {nameof(AppUserModel.Roles)}
        //    FROM [identity].Users AS U
        //    """;

        var sql = $"""
                SELECT 
                    U.Id AS {nameof(AppUserModel.Id)}, 
                    U.FirstName AS {nameof(AppUserModel.FirstName)}, 
                    U.LastName AS {nameof(AppUserModel.LastName)}, 
                    U.Username AS {nameof(AppUserModel.Username)}, 
                    U.Email AS {nameof(AppUserModel.Email)} , 
                    U.PhoneNumber AS {nameof(AppUserModel.PhoneNumber)},
                    IIF(U.IsActive = 1, 'Active', 'Inactive') AS {nameof(AppUserModel.Status)},
                    U.PhotoUrl AS {nameof(AppUserModel.PhotoUrl)},
                    STRING_AGG(R.Name, ', ') AS {nameof(AppUserModel.AssignedRoles)}
                FROM [identity].Users AS U
                LEFT JOIN [identity].UserRoles AS UR ON UR.UserId = U.Id
                LEFT JOIN [identity].Roles AS R ON R.Id = UR.RoleId
                """;

        string groupBy = """
            GROUP BY 
                U.Id, U.FirstName, U.LastName, U.Username, 
                U.Email, U.PhoneNumber, U.IsActive, U.PhotoUrl
            """;

        return await PaginatedResponse<AppUserModel>
            .CreateAsync(connection, sql, request, groupBy: groupBy);

    }
}
