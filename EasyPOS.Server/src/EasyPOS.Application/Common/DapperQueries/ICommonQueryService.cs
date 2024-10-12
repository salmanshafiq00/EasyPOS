using EasyPOS.Application.Features.LookupDetails.Queries;

namespace EasyPOS.Application.Common.DapperQueries;

public interface ICommonQueryService
{
    Task<bool> IsExistAsync(string tableName, string[] equalFilters, object? param = null, string[]? notEqualFilters = null);
    Task<Guid?> GetLookupDetailIdAsync(int lookupDetailDevCode);
    Task<List<LookupDetailModel>> GetLookupDetailsAsync(int lookupDetailDevCode);
}
