namespace EasyPOS.Application.Features.Trades.PurchasePayments.Commands;

public record DeletePurchasePaymentsCommand(Guid[] Ids): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.PurchasePayment;
}

internal sealed class DeletePurchasePaymentsCommandHandler(
    ISqlConnectionFactory sqlConnection) 
    : ICommandHandler<DeletePurchasePaymentsCommand>

{
    public async Task<Result> Handle(DeletePurchasePaymentsCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            Delete dbo.PurchasePayments
            WHERE Id IN @Id
            """;

        await connection.ExecuteAsync(sql, new { request.Ids });

        return Result.Success();
    }

}