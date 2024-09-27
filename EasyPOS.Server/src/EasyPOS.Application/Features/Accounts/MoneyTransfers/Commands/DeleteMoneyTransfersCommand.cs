namespace EasyPOS.Application.Features.Accounts.MoneyTransfers.Commands;

public record DeleteMoneyTransfersCommand(Guid[] Ids): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.MoneyTransfer;
}

internal sealed class DeleteMoneyTransfersCommandHandler(
    ISqlConnectionFactory sqlConnection) 
    : ICommandHandler<DeleteMoneyTransfersCommand>

{
    public async Task<Result> Handle(DeleteMoneyTransfersCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            Delete dbo.MoneyTransfers
            WHERE Id IN @Id
            """;

        await connection.ExecuteAsync(sql, new { request.Ids });

        return Result.Success();
    }

}