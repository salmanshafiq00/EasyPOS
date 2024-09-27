namespace EasyPOS.Application.Features.Accounts.MoneyTransfers.Queries;

[Authorize(Policy = Permissions.MoneyTransfers.View)]
public record GetMoneyTransfersListQuery 
     : DataGridModel, ICacheableQuery<PaginatedResponse<MoneyTransferModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.MoneyTransfer}_{PageNumber}_{PageSize}";
     
}

internal sealed class GetMoneyTransfersQueryHandler(ISqlConnectionFactory sqlConnection) 
     : IQueryHandler<GetMoneyTransfersListQuery, PaginatedResponse<MoneyTransferModel>>
{
    public async Task<Result<PaginatedResponse<MoneyTransferModel>>> Handle(GetMoneyTransfersListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(MoneyTransferModel.Id)},
                t.FromAccountId AS {nameof(MoneyTransferModel.FromAccountId)},
                t.ToAccountId AS {nameof(MoneyTransferModel.ToAccountId)},
                t.Amount AS {nameof(MoneyTransferModel.Amount)}
            FROM dbo.MoneyTransfers AS t
            WHERE 1 = 1
            """;


        return await PaginatedResponse<MoneyTransferModel>
            .CreateAsync(connection, sql, request);

    }
}


