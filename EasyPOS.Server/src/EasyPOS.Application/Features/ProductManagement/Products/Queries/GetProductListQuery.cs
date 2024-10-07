namespace EasyPOS.Application.Features.ProductManagement.Queries;

[Authorize(Policy = Permissions.Products.View)]
public record GetProductListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<ProductModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Product}_{PageNumber}_{PageSize}";
}

internal sealed class GetProductListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetProductListQuery, PaginatedResponse<ProductModel>>
{
    public async Task<Result<PaginatedResponse<ProductModel>>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        string sql = $"""
            SELECT
                t.Id AS {nameof(ProductModel.Id)},
                t.Name AS {nameof(ProductModel.Name)},
                t.CategoryId AS {nameof(ProductModel.CategoryId)},
                t.ProductTypeId AS {nameof(ProductModel.ProductTypeId)},
                t.BrandId AS {nameof(ProductModel.BrandId)},
                t.Code AS {nameof(ProductModel.Code)},
                t.SKU AS {nameof(ProductModel.SKU)},
                t.CostPrice AS {nameof(ProductModel.CostPrice)},
                t.SalePrice AS {nameof(ProductModel.SalePrice)},
                t.WholesalePrice AS {nameof(ProductModel.WholesalePrice)},
                t.Unit AS {nameof(ProductModel.Unit)},
                t.SaleUnit AS {nameof(ProductModel.SaleUnit)},
                t.PurchaseUnit AS {nameof(ProductModel.PurchaseUnit)},
                t.AlertQuantity AS {nameof(ProductModel.AlertQuantity)},
                t.BarCodeType AS {nameof(ProductModel.BarCodeType)},
                t.QRCodeType AS {nameof(ProductModel.QRCodeType)},
                t.Description AS {nameof(ProductModel.Description)},
                t.IsActive AS {nameof(ProductModel.IsActive)}
            FROM dbo.Products t
            """;

        var sqlWithOrders = $"""
            {sql} 
            ORDER BY t.Name
            """;

        return await PaginatedResponse<ProductModel>
            .CreateAsync(connection, sql, request);
    }
}
