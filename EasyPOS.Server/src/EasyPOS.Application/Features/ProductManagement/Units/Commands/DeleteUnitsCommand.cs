namespace EasyPOS.Application.Features.ProductManagement.Units.Commands;

public record DeleteUnitsCommand(Guid[] Ids) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Unit;
}

internal sealed class DeleteUnitsCommandHandler(
    ISqlConnectionFactory sqlConnection)
    : ICommandHandler<DeleteUnitsCommand>

{
    public async Task<Result> Handle(DeleteUnitsCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            Delete dbo.Units
            WHERE Id IN @Id
            """;

        await connection.ExecuteAsync(sql, new { request.Ids });

        return Result.Success();
    }

}
