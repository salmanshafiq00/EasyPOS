namespace EasyPOS.Application.Common.DapperQueries;

public static class DapperPaginatedQueryHelper
{
    public const string Offset = nameof(Offset);

    public const string Next = nameof(Next);

    public static DapperPaginatedData GetPaginatedData(IDapperPaginatedQuery query)
    {
        int offset = !query.PageNumber.HasValue || !query.PageSize.HasValue
            ? 0
            : (query.PageNumber.Value - 1) * query.PageSize.Value;

        int next = query.PageSize ?? int.MaxValue;

        return new DapperPaginatedData(offset, next);
    }

    public static string AppendPageStatement(string sql)
    {
        return $"{sql} " +
               $"OFFSET @{Offset} ROWS FETCH Next @{Next} ROWS ONLY; ";
    }
}
