namespace EasyPOS.Application.Features.Trades.PurchasePayments.Queries;

public record GetPurchasePaymentByIdQuery(Guid Id) : ICacheableQuery<PurchasePaymentModel>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.PurchasePayment}_{Id}";
    [JsonIgnore]
    public TimeSpan? Expiration => null;
    public bool? AllowCache => true;
}

internal sealed class GetPurchasePaymentByIdQueryHandler(ISqlConnectionFactory sqlConnection)
     : IQueryHandler<GetPurchasePaymentByIdQuery, PurchasePaymentModel>
{

    public async Task<Result<PurchasePaymentModel>> Handle(GetPurchasePaymentByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new PurchasePaymentModel();
        }
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(PurchasePaymentModel.Id)},
                t.PurchaseId AS {nameof(PurchasePaymentModel.PurchaseId)},
                t.PaymentDate AS {nameof(PurchasePaymentModel.PaymentDate)},
                t.ReceivedAmount AS {nameof(PurchasePaymentModel.ReceivedAmount)},
                t.PayingAmount AS {nameof(PurchasePaymentModel.PayingAmount)},
                t.ChangeAmount AS {nameof(PurchasePaymentModel.ChangeAmount)},
                t.PaymentType AS {nameof(PurchasePaymentModel.PaymentType)},
                t.Note AS {nameof(PurchasePaymentModel.Note)}
            FROM dbo.PurchasePayments AS t
            WHERE t.Id = @Id
            """;


        return await connection.QueryFirstOrDefaultAsync<PurchasePaymentModel>(sql, new { request.Id });
    }
}

