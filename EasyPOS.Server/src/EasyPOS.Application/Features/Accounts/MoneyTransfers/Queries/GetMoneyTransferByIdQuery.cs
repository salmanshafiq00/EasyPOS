namespace EasyPOS.Application.Features.Accounts.MoneyTransfers.Queries;

public record GetMoneyTransferByIdQuery(Guid Id) : ICacheableQuery<MoneyTransferModel>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.MoneyTransfer}_{Id}";
    [JsonIgnore]
    public TimeSpan? Expiration => null;
    public bool? AllowCache => true;
}

internal sealed class GetMoneyTransferByIdQueryHandler(ISqlConnectionFactory sqlConnection)
     : IQueryHandler<GetMoneyTransferByIdQuery, MoneyTransferModel>
{

    public async Task<Result<MoneyTransferModel>> Handle(GetMoneyTransferByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new MoneyTransferModel();
        }
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(MoneyTransferModel.Id)},
                t.FromAccountId AS {nameof(MoneyTransferModel.FromAccountId)},
                t.ToAccountId AS {nameof(MoneyTransferModel.ToAccountId)},
                t.Amount AS {nameof(MoneyTransferModel.Amount)}
            FROM dbo.MoneyTransfers AS t
            WHERE t.Id = @Id
            """;


        return await connection.QueryFirstOrDefaultAsync<MoneyTransferModel>(sql, new { request.Id });
    }
}

