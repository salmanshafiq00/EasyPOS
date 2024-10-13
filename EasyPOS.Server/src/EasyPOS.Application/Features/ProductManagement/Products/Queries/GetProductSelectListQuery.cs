namespace EasyPOS.Application.Features.ProductManagement.Queries;

public record GetProductSelectListQuery(
    bool? AllowCacheList) : ICacheableQuery<List<ProductSelectListModel>>
{
    public TimeSpan? Expiration => null;
    [JsonIgnore]
    public string CacheKey => CacheKeys.Product_All_SelectList;
    public bool? AllowCache => AllowCacheList ?? true;

}

internal sealed class ProductSelectListQueryHandler(
    ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetProductSelectListQuery, List<ProductSelectListModel>>
{
    public async Task<Result<List<ProductSelectListModel>>> Handle(GetProductSelectListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        string sql = $"""
            SELECT 
                t.Id AS {nameof(ProductSelectListModel.Id)},
                t.Name AS {nameof(ProductSelectListModel.Name)},
                t.Code AS {nameof(ProductSelectListModel.Code)},
                t.PurchaseUnit AS {nameof(ProductSelectListModel.PurchaseUnit)},
                t.SaleUnit AS {nameof(ProductSelectListModel.SaleUnit)},
                t.CostPrice AS {nameof(ProductSelectListModel.CostPrice)},
                t.SalePrice AS {nameof(ProductSelectListModel.SalePrice)},
                t.TaxRate AS {nameof(ProductSelectListModel.TaxRate)},
                t.TaxMethod AS {nameof(ProductSelectListModel.TaxMethod)},
                t.DiscountType AS {nameof(ProductSelectListModel.DiscountType)},
                t.DiscountRate AS {nameof(ProductSelectListModel.DiscountRate)},
                t.DiscountAmount AS {nameof(ProductSelectListModel.DiscountAmount)}
            FROM dbo.Products t
            WHERE 1 = 1
            ORDER BY t.Name
            """;


        var selectList = await connection
                .QueryAsync<ProductSelectListModel>(sql);

        return Result.Success(selectList.AsList());
    }
}


