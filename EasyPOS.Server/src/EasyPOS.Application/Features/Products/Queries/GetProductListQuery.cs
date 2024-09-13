namespace EasyPOS.Application.Features.Products.Queries;

[Authorize(Policy = Permissions.Products.View)]
public record GetProductListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<ProductModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Product}l_{PageNumber}_{PageSize}";
}

internal sealed class GetProductListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetProductListQuery, PaginatedResponse<ProductModel>>
{
    public async Task<Result<PaginatedResponse<ProductModel>>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        string sql = $"""
            SELECT
                p.Id AS {nameof(ProductModel.Id)},
                p.Name AS {nameof(ProductModel.Name)},
                p.CategoryId AS {nameof(ProductModel.CategoryId)},
                p.ProductTypeId AS {nameof(ProductModel.ProductTypeId)},
                p.BrandId AS {nameof(ProductModel.BrandId)},
                p.Code AS {nameof(ProductModel.Code)},
                p.SKU AS {nameof(ProductModel.SKU)},
                p.CostPrice AS {nameof(ProductModel.CostPrice)},
                p.Price AS {nameof(ProductModel.Price)},
                p.WholesalePrice AS {nameof(ProductModel.WholesalePrice)},
                p.Unit AS {nameof(ProductModel.Unit)},
                p.SaleUnit AS {nameof(ProductModel.SaleUnit)},
                p.PurchaseUnit AS {nameof(ProductModel.PurchaseUnit)},
                p.AlertQuantity AS {nameof(ProductModel.AlertQuantity)},
                p.BarCodeType AS {nameof(ProductModel.BarCodeType)},
                p.QRCodeType AS {nameof(ProductModel.QRCodeType)},
                p.Description AS {nameof(ProductModel.Description)},
                p.IsActive AS {nameof(ProductModel.IsActive)}
            FROM dbo.Products p
            """;

        var sqlWithOrders = $"""
            {sql} 
            ORDER BY p.Name
            """;

        return await PaginatedResponse<ProductModel>
            .CreateAsync(connection, sql, request);
    }
}
