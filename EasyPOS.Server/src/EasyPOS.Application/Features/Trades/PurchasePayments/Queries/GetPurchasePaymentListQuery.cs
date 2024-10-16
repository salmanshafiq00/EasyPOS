namespace EasyPOS.Application.Features.Trades.PurchasePayments.Queries;

[Authorize(Policy = Permissions.PurchasePayments.View)]
public record GetPurchasePaymentListQuery 
     : DataGridModel, ICacheableQuery<PaginatedResponse<PurchasePaymentModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.PurchasePayment}_{PageNumber}_{PageSize}";
    public Guid PurchaseId { get; set; }

}

internal sealed class GetPurchasePaymentQueryHandler(ISqlConnectionFactory sqlConnection) 
     : IQueryHandler<GetPurchasePaymentListQuery, PaginatedResponse<PurchasePaymentModel>>
{
    public async Task<Result<PaginatedResponse<PurchasePaymentModel>>> Handle(GetPurchasePaymentListQuery request, CancellationToken cancellationToken)
    {
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
            WHERE 1 = 1
            AND t.PurchaseId = @PurchaseId
            """;


        return await PaginatedResponse<PurchasePaymentModel>
            .CreateAsync(connection, sql, request, request.PurchaseId);

    }
}


