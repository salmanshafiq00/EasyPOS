namespace EasyPOS.Application.Features.Trades.Sales.Commands;

public record DeleteSalesCommand(Guid[] Ids): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Sale;
}

internal sealed class DeleteSalesCommandHandler(
    ISqlConnectionFactory sqlConnection) 
    : ICommandHandler<DeleteSalesCommand>

{
    public async Task<Result> Handle(DeleteSalesCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            Delete dbo.Sales
            WHERE Id IN @Id
            """;

        await connection.ExecuteAsync(sql, new { request.Ids });

        return Result.Success();
    }

}