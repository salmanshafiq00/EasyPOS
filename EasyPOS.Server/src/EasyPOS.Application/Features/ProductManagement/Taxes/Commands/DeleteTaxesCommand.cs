namespace EasyPOS.Application.Features.ProductManagement.Taxes.Commands;

public record DeleteTaxesCommand(Guid[] Ids) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Tax;
}

internal sealed class DeleteTaxesCommandHandler(
    ISqlConnectionFactory sqlConnection)
    : ICommandHandler<DeleteTaxesCommand>

{
    public async Task<Result> Handle(DeleteTaxesCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            Delete dbo.Taxes
            WHERE Id IN @Id
            """;

        await connection.ExecuteAsync(sql, new { request.Ids });

        return Result.Success();
    }

}
