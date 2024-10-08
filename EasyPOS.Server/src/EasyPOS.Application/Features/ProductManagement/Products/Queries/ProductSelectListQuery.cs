﻿namespace EasyPOS.Application.Features.ProductManagement.Queries;

public record ProductSelectListQuery(
    bool? AllowCacheList) : ICacheableQuery<List<ProductSelectListModel>>
{
    public TimeSpan? Expiration => null;
    [JsonIgnore]
    public string CacheKey => CacheKeys.Product_All_SelectList;
    public bool? AllowCache => AllowCacheList ?? true;

}

internal sealed class ProductSelectListQueryHandler(
    ISqlConnectionFactory sqlConnection)
    : IQueryHandler<ProductSelectListQuery, List<ProductSelectListModel>>
{
    public async Task<Result<List<ProductSelectListModel>>> Handle(ProductSelectListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        string sql = """
            SELECT 
                Id, 
                Name, 
                Code, 
                CostPrice, 
                SalePrice,
                TaxRate,
                TaxMethod,
                Discount
            FROM dbo.Products t
            WHERE 1 = 1
            ORDER BY Name
            """;

        var selectList = await connection
                .QueryAsync<ProductSelectListModel>(sql);

        return Result.Success(selectList.AsList());
    }
}


