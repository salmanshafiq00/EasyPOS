namespace EasyPOS.Application.Common.DapperQueries;

public struct DapperPaginatedData(int offset, int next)
{
    public int Offset { get; } = offset;

    public int Next { get; } = next;
}
