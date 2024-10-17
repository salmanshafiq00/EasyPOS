namespace EasyPOS.Application.Features.Trades.PurchasePayments.Queries;

[Authorize(Policy = Permissions.PurchasePayments.View)]
public record GetPaymentListByPurchaseIdQuery
     : ICacheableQuery<List<PurchasePaymentModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.PurchasePayment}_{PurchaseId}";
    public Guid PurchaseId { get; set; }
    public bool? AllowCache => false;
    [JsonIgnore]
    public TimeSpan? Expiration => null;
}

internal sealed class GetPaymentListByPurchaseIdQueryHandler(ISqlConnectionFactory sqlConnection) 
     : IQueryHandler<GetPaymentListByPurchaseIdQuery, List<PurchasePaymentModel>>
{
    public async Task<Result<List<PurchasePaymentModel>>> Handle(GetPaymentListByPurchaseIdQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(PurchasePaymentModel.Id)},
                t.PurchaseId AS {nameof(PurchasePaymentModel.PurchaseId)},
                t.PaymentDate AS {nameof(PurchasePaymentModel.PaymentDate)},
                -- CAST(t.PaymentDate AS DATE) AS {nameof(PurchasePaymentModel.PaymentDateString)},
                FORMAT(t.PaymentDate, 'dd/MM/yyyy') AS {nameof(PurchasePaymentModel.PaymentDateString)},
                --FORMAT(t.PaymentDate, 'dd/MM/yyyy HH:mm:ss') AS {nameof(PurchasePaymentModel.PaymentDateString)},
                t.ReceivedAmount AS {nameof(PurchasePaymentModel.ReceivedAmount)},
                t.PayingAmount AS {nameof(PurchasePaymentModel.PayingAmount)},
                t.ChangeAmount AS {nameof(PurchasePaymentModel.ChangeAmount)},
                t.PaymentType AS {nameof(PurchasePaymentModel.PaymentType)},
                t.Note AS {nameof(PurchasePaymentModel.Note)},
                ld.Name AS {nameof(PurchasePaymentModel.PaymentTypeName)},
                u.Username AS {nameof(PurchasePaymentModel.CreatedBy)}
            FROM dbo.PurchasePayments AS t
            LEFT JOIN dbo.LookupDetails ld ON ld.Id = t.PaymentType
            LEFT JOIN [identity].Users u ON u.Id = t.CreatedBy
            WHERE 1 = 1
            AND t.PurchaseId = @PurchaseId
            """;


        var result = await connection.QueryAsync<PurchasePaymentModel>(sql, new { PurchaseId = request.PurchaseId });
        return result.AsList();

    }
}


