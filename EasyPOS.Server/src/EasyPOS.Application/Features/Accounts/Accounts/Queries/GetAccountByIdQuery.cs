namespace EasyPOS.Application.Features.Accounts.Accounts.Queries;

public record GetAccountByIdQuery(Guid Id) : ICacheableQuery<AccountModel>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.Account}_{Id}";
    [JsonIgnore]
    public TimeSpan? Expiration => null;
    public bool? AllowCache => true;
}

internal sealed class GetAccountByIdQueryHandler(ISqlConnectionFactory sqlConnection)
     : IQueryHandler<GetAccountByIdQuery, AccountModel>
{

    public async Task<Result<AccountModel>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new AccountModel();
        }
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(AccountModel.Id)},
                t.AccountNo AS {nameof(AccountModel.AccountNo)},
                t.Name AS {nameof(AccountModel.Name)},
                t.Balance AS {nameof(AccountModel.Balance)},
                t.Note AS {nameof(AccountModel.Note)}
            FROM dbo.Accounts AS t
            WHERE t.Id = @Id
            """;


        return await connection.QueryFirstOrDefaultAsync<AccountModel>(sql, new { request.Id });
    }
}

