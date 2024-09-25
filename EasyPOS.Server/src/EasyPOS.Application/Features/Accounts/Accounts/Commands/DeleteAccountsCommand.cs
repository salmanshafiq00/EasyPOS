namespace EasyPOS.Application.Features.Accounts.Accounts.Commands;

public record DeleteAccountsCommand(Guid[] Ids): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Account;
}

internal sealed class DeleteAccountsCommandHandler(
    ISqlConnectionFactory sqlConnection) 
    : ICommandHandler<DeleteAccountsCommand>

{
    public async Task<Result> Handle(DeleteAccountsCommand request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            Delete dbo.Accounts
            WHERE Id IN @Id
            """;

        await connection.ExecuteAsync(sql, new { request.Ids });

        return Result.Success();
    }

}