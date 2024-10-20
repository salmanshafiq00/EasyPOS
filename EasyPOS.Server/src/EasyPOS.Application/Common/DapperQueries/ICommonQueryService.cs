using EasyPOS.Application.Features.Settings.CompanyInfos.Queries;
using EasyPOS.Application.Features.Suppliers.Queries;
using EasyPOS.Domain.Common;

namespace EasyPOS.Application.Common.DapperQueries;

public interface ICommonQueryService
{
    Task<bool> IsExistAsync(string tableName, string[] equalFilters, object? param = null, string[]? notEqualFilters = null);
    Task<Guid?> GetLookupDetailIdAsync(int lookupDetailDevCode, CancellationToken cancellationToken = default);
    Task<List<LookupDetail>> GetLookupDetailsAsync(int lookupDevCode, CancellationToken cancellationToken = default);
    Task<CompanyInfoModel> GetCompanyInfoAsync(CancellationToken cancellationToken = default);
    Task<SupplierModel> GetSupplierDetail(Guid supplierId, CancellationToken cancellationToken = default);
    Task<SupplierModel> GetCustomerDetail(Guid customerId, CancellationToken cancellationToken = default);
}
