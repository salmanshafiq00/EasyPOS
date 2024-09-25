namespace EasyPOS.Application.Features.Accounts.Accounts.Queries;

[Authorize(Policy = Permissions.Accounts.View)]
public record GetAccountsListQuery 
     : DataGridModel, ICacheableQuery<PaginatedResponse<AccountModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Account}_{PageNumber}_{PageSize}";
     
}

internal sealed class GetAccountsQueryHandler(ISqlConnectionFactory sqlConnection) 
     : IQueryHandler<GetAccountsListQuery, PaginatedResponse<AccountModel>>
{
    public async Task<Result<PaginatedResponse<AccountModel>>> Handle(GetAccountsListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(AccountModel.Id)},
                t.AccountNo AS {nameof(AccountModel.AccountNo)},
                t.Name AS {nameof(AccountModel.Name)},
                t.Balance AS {nameof(AccountModel.Balance)},
                t.Note AS {nameof(AccountModel.Note)}
            FROM dbo.Accounts AS t
            WHERE 1 = 1
            """;


        return await PaginatedResponse<AccountModel>
            .CreateAsync(connection, sql, request);

    }
}


