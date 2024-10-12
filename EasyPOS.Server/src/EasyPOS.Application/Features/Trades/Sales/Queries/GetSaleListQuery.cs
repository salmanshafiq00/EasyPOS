namespace EasyPOS.Application.Features.Trades.Sales.Queries;

[Authorize(Policy = Permissions.Sales.View)]
public record GetSaleListQuery
     : DataGridModel, ICacheableQuery<PaginatedResponse<SaleModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Sale}_{PageNumber}_{PageSize}";
}

internal sealed class GetSaleQueryHandler(ISqlConnectionFactory sqlConnection) 
     : IQueryHandler<GetSaleListQuery, PaginatedResponse<SaleModel>>
{
    public async Task<Result<PaginatedResponse<SaleModel>>> Handle(GetSaleListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(SaleModel.Id)},
                t.SaleDate As {nameof(SaleModel.SaleDate)},
                t.ReferenceNo AS {nameof(SaleModel.ReferenceNo)},
                t.WarehouseId AS {nameof(SaleModel.WarehouseId)},
                t.CustomerId AS {nameof(SaleModel.CustomerId)},
                t.AttachmentUrl AS {nameof(SaleModel.AttachmentUrl)},
                t.SaleStatusId AS {nameof(SaleModel.SaleStatusId)},
                t.PaymentStatusId AS {nameof(SaleModel.PaymentStatusId)},
                t.TaxRate AS {nameof(SaleModel.TaxRate)},
                t.TaxAmount AS {nameof(SaleModel.TaxAmount)},
                t.DiscountAmount AS {nameof(SaleModel.DiscountAmount)},
                t.ShippingCost AS {nameof(SaleModel.ShippingCost)},
                t.GrandTotal AS {nameof(SaleModel.GrandTotal)},
                t.SaleNote AS {nameof(SaleModel.SaleNote)},
                t.StaffNote AS {nameof(SaleModel.StaffNote)},
                w.Name AS {nameof(SaleModel.WarehouseName)},
                c.Name AS {nameof(SaleModel.CustomerName)},
                ss.Name AS {nameof(SaleModel.SaleStatus)},
                ps.Name AS {nameof(SaleModel.PaymentStatus)}
            FROM dbo.Sales AS t
            LEFT JOIN dbo.Warehouses w ON w.Id = t.WarehouseId
            LEFT JOIN dbo.Customers c ON c.Id = t.CustomerId
            LEFT JOIN dbo.LookupDetails ss ON ss.Id = t.SaleStatusId
            LEFT JOIN dbo.LookupDetails ps ON ps.Id = t.PaymentStatusId
            WHERE 1 = 1
            """;


        return await PaginatedResponse<SaleModel>
            .CreateAsync(connection, sql, request);

    }
}


