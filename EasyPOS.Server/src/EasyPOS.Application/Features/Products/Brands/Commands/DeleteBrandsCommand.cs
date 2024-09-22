namespace EasyPOS.Application.Features.Brands.Commands;

public record DeleteBrandsCommand(Guid[] Ids) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Brand;
}

internal sealed class DeleteBrandsCommandHandler(
    ISqlConnectionFactory sqlConnection)
    : ICommandHandler<DeleteBrandsCommand>
{
    public async Task<Result> Handle(DeleteBrandsCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            Delete dbo.Brands
            WHERE Id IN @Id
            """;

        await connection.ExecuteAsync(sql, new { Ids = request.Ids });

        return Result.Success();
    }
}
