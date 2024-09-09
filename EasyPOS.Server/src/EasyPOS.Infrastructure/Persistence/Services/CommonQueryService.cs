using System.Text;
using Dapper;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.DapperQueries;

namespace EasyPOS.Infrastructure.Persistence.Services;

internal sealed class CommonQueryService(ISqlConnectionFactory sqlConnection) : ICommonQueryService
{
    public async Task<bool> IsExist(
        string tableName,
        string[]? equalFilters,
        object? param = null,
        string[]? notEqualFilters = null)
    {
        var connection = sqlConnection.GetOpenConnection();

        StringBuilder sql = new($"SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM {tableName} WHERE 1 = 1");

        foreach (var filter in equalFilters ??= [])
        {
            sql.Append($" AND {filter} = @{filter}");
        }

        foreach (var filter in notEqualFilters ??= [])
        {
            sql.Append($" AND {filter} <> @{filter}");
        }

        sql.Append(") THEN 1 ELSE 0 END AS BIT)");

        return await connection.ExecuteScalarAsync<bool>(sql.ToString(), param);
    }

}
