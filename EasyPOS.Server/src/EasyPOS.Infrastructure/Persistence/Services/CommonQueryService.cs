using System.Text;
using Azure.Core;
using Dapper;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.DapperQueries;
using EasyPOS.Application.Features.Settings.CompanyInfos.Queries;
using EasyPOS.Application.Features.Suppliers.Queries;
using EasyPOS.Domain.Common;

namespace EasyPOS.Infrastructure.Persistence.Services;

internal sealed class CommonQueryService(ISqlConnectionFactory sqlConnection) : ICommonQueryService
{
    public async Task<Guid?> GetLookupDetailIdAsync(
        int lookupDetailDevCode, 
        CancellationToken cancellationToken = default)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = """
            SELECT TOP 1 Id
            FROM dbo.LookupDetails
            WHERE DevCode = @DevCode
            """;
        return await connection.QueryFirstOrDefaultAsync<Guid?>(sql, new {DevCode = lookupDetailDevCode});
    }

    public async Task<List<LookupDetail>> GetLookupDetailsAsync(
        int lookupDevCode, 
        CancellationToken cancellationToken = default)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = """
            SELECT 
                ld.Id, 
                ld.Name, 
                ld.Description, 
                ld.Status, 
                ld.ParentId, 
                ld.LookupId, 
                ld.DevCode
            FROM 
                dbo.Lookups l
            INNER JOIN 
                dbo.LookupDetails ld ON l.Id = ld.LookupId
            WHERE 
                l.DevCode = @DevCode          
            """;

        var result = await connection.QueryAsync<LookupDetail>(sql, new { DevCode = lookupDevCode });

        return result.AsList();
    }

    public async Task<SupplierModel> GetSupplierDetail(Guid supplierId, CancellationToken cancellationToken = default)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(SupplierModel.Id)},
                t.Name AS {nameof(SupplierModel.Name)},
                t.Email AS {nameof(SupplierModel.Email)},
                t.PhoneNo AS {nameof(SupplierModel.PhoneNo)},
                t.Mobile AS {nameof(SupplierModel.Mobile)},
                t.Country AS {nameof(SupplierModel.Country)},
                t.City AS {nameof(SupplierModel.City)},
                t.Address AS {nameof(SupplierModel.Address)},
                t.IsActive AS {nameof(SupplierModel.IsActive)}
            FROM dbo.Suppliers t
            WHERE t.Id = @Id
            """
        ;
        return await connection.QueryFirstOrDefaultAsync<SupplierModel>(sql, new {Id = supplierId });
    }

    public async Task<SupplierModel> GetCustomerDetail(Guid customerId, CancellationToken cancellationToken = default)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(SupplierModel.Id)},
                t.Name AS {nameof(SupplierModel.Name)},
                t.Email AS {nameof(SupplierModel.Email)},
                t.PhoneNo AS {nameof(SupplierModel.PhoneNo)},
                t.Mobile AS {nameof(SupplierModel.Mobile)},
                t.Country AS {nameof(SupplierModel.Country)},
                t.City AS {nameof(SupplierModel.City)},
                t.Address AS {nameof(SupplierModel.Address)},
                t.IsActive AS {nameof(SupplierModel.IsActive)}
            FROM dbo.Customers t
            WHERE t.Id = @Id
            """
        ;
        return await connection.QueryFirstOrDefaultAsync<SupplierModel>(sql, new { Id = customerId });
    }

    public async Task<CompanyInfoModel> GetCompanyInfoAsync(
        CancellationToken cancellationToken = default)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT TOP 1
                t.Id AS {nameof(CompanyInfoModel.Id)},
                t.Name AS {nameof(CompanyInfoModel.Name)},
                t.Phone AS {nameof(CompanyInfoModel.Phone)},
                t.Mobile AS {nameof(CompanyInfoModel.Mobile)},
                t.Country AS {nameof(CompanyInfoModel.Country)},
                t.State AS {nameof(CompanyInfoModel.State)},
                t.City AS {nameof(CompanyInfoModel.City)},
                t.PostalCode AS {nameof(CompanyInfoModel.PostalCode)},
                t.Address AS {nameof(CompanyInfoModel.Address)},
                t.LogoUrl AS {nameof(CompanyInfoModel.LogoUrl)},
                t.SignatureUrl AS {nameof(CompanyInfoModel.SignatureUrl)},
                t.Website AS {nameof(CompanyInfoModel.Website)}
            FROM dbo.CompanyInfos AS t
            """;

        return await connection.QueryFirstOrDefaultAsync<CompanyInfoModel>(sql);
    }

    public async Task<bool> IsExistAsync(
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
