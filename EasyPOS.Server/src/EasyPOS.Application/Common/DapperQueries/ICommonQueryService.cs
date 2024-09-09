namespace EasyPOS.Application.Common.DapperQueries;

public interface ICommonQueryService
{
    Task<bool> IsExist(string tableName, string[] equalFilters, object? param = null, string[]? notEqualFilters = null);
}
