namespace EasyPOS.Application.Features.ProductManagement.Queries;

public record GetProductByIdQuery(Guid Id) : ICacheableQuery<ProductModel>
{
    public string CacheKey => $"{CacheKeys.Product}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => false;
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
                t.TaxRate AS {nameof(ProductModel.TaxRate)},
                t.TaxMethod AS {nameof(ProductModel.TaxMethod)},
                t.IsActive AS {nameof(ProductModel.IsActive)}
            FROM dbo.Products t
            WHERE t.Id = @Id
            """;


        return await connection.QueryFirstOrDefaultAsync<ProductModel>(sql, new { request.Id });
    }
}
