namespace EasyPOS.Application.Features.Settings.CompanyInfos.Commands;

public record DeleteCompanyInfosCommand(Guid[] Ids): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.CompanyInfo;
}

internal sealed class DeleteCompanyInfosCommandHandler(
    ISqlConnectionFactory sqlConnection) 
    : ICommandHandler<DeleteCompanyInfosCommand>

{
    public async Task<Result> Handle(DeleteCompanyInfosCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            Delete dbo.CompanyInfos
            WHERE Id IN @Id
            """;

        await connection.ExecuteAsync(sql, new { request.Ids });

        return Result.Success();
    }

}