namespace EasyPOS.Application.Features.Products.Queries;

public record GetProductByIdQuery(Guid Id) : ICacheableQuery<ProductModel>
{
    public string CacheKey => $"{CacheKeys.Product}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => true;
}

internal sealed class GetProductByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) 
    : IQueryHandler<GetProductByIdQuery, ProductModel>
{
    public async Task<Result<ProductModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new ProductModel();
        }
        var connection = sqlConnectionFactory.GetOpenConnection();

        string sql = $"""
            SELECT
                c.Id AS {nameof(ProductModel.Id)},
                c.Name AS {nameof(ProductModel.Name)},
                c.CategoryId AS {nameof(ProductModel.CategoryId)},
                c.ProductTypeId AS {nameof(ProductModel.ProductTypeId)},
                c.BrandId AS {nameof(ProductModel.BrandId)},
                c.Code AS {nameof(ProductModel.Code)},
                c.SKU AS {nameof(ProductModel.SKU)},
                c.CostPrice AS {nameof(ProductModel.CostPrice)},
                c.SalePrice AS {nameof(ProductModel.Price)},
                c.WholesalePrice AS {nameof(ProductModel.WholesalePrice)},
                c.Unit AS {nameof(ProductModel.Unit)},
                c.SaleUnit AS {nameof(ProductModel.SaleUnit)},
                c.PurchaseUnit AS {nameof(ProductModel.PurchaseUnit)},
                c.AlertQuantity AS {nameof(ProductModel.AlertQuantity)},
                c.BarCodeType AS {nameof(ProductModel.BarCodeType)},
                c.QRCodeType AS {nameof(ProductModel.QRCodeType)},
                c.Description AS {nameof(ProductModel.Description)},
                c.IsActive AS {nameof(ProductModel.IsActive)},
            FROM dbo.Products c
            WHERE c.Id = @Id
            """;


        return await connection.QueryFirstOrDefaultAsync<ProductModel>(sql, new { request.Id });
    }
}
