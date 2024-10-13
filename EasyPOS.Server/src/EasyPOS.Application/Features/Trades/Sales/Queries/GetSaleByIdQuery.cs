using EasyPOS.Application.Common.Enums;
using EasyPOS.Domain.Enums;

namespace EasyPOS.Application.Features.Trades.Sales.Queries;

public record GetSaleByIdQuery(Guid Id) : ICacheableQuery<UpsertSaleModel>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.Sale}_{Id}";
    [JsonIgnore]
    public TimeSpan? Expiration => null;
    public bool? AllowCache => false;
}

internal sealed class GetSaleByIdQueryHandler(ISqlConnectionFactory sqlConnection, ICommonQueryService commonQueryService)
    : IQueryHandler<GetSaleByIdQuery, UpsertSaleModel>
{
    public async Task<Result<UpsertSaleModel>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new UpsertSaleModel()
            {
                DiscountType = DiscountType.Fixed,
                ShippingCost = 0,
                DiscountAmount = 0,
                SaleStatusId = await commonQueryService.GetLookupDetailIdAsync((int)SaleSatus.Completed) ,
                PaymentStatusId = await commonQueryService.GetLookupDetailIdAsync((int)PaymentStatus.Pending),
                TaxRate = 0
            };
        }

        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                -- UpsertSaleModel fields (master)
                s.Id AS {nameof(UpsertSaleModel.Id)},
                s.SaleDate AS {nameof(UpsertSaleModel.SaleDate)},
                s.ReferenceNo AS {nameof(UpsertSaleModel.ReferenceNo)},
                s.WarehouseId AS {nameof(UpsertSaleModel.WarehouseId)},
                s.CustomerId AS {nameof(UpsertSaleModel.CustomerId)},
                s.BillerId AS {nameof(UpsertSaleModel.BillerId)},
                s.AttachmentUrl AS {nameof(UpsertSaleModel.AttachmentUrl)},
                s.SaleStatusId AS {nameof(UpsertSaleModel.SaleStatusId)},
                s.PaymentStatusId AS {nameof(UpsertSaleModel.PaymentStatusId)},
                s.SubTotal AS {nameof(UpsertSaleModel.SubTotal)},
                s.TaxRate AS {nameof(UpsertSaleModel.TaxRate)},
                s.TaxAmount AS {nameof(UpsertSaleModel.TaxAmount)},
                s.DiscountType AS {nameof(UpsertSaleModel.DiscountType)},
                s.DiscountRate AS {nameof(UpsertSaleModel.DiscountRate)},
                s.DiscountAmount AS {nameof(UpsertSaleModel.DiscountAmount)},
                s.ShippingCost AS {nameof(UpsertSaleModel.ShippingCost)},
                s.GrandTotal AS {nameof(UpsertSaleModel.GrandTotal)},
                s.SaleNote AS {nameof(UpsertSaleModel.SaleNote)},
                s.StaffNote AS {nameof(UpsertSaleModel.StaffNote)},

                -- SaleDetailModel fields (detail)
                d.Id AS {nameof(SaleDetailModel.Id)},
                d.SaleId AS {nameof(SaleDetailModel.SaleId)},
                d.ProductId AS {nameof(SaleDetailModel.ProductId)},
                d.ProductCode AS {nameof(SaleDetailModel.ProductCode)},
                d.ProductName AS {nameof(SaleDetailModel.ProductName)},
                d.ProductUnitCost AS {nameof(SaleDetailModel.ProductUnitCost)},
                d.ProductUnitPrice AS {nameof(SaleDetailModel.ProductUnitPrice)},
                d.ProductUnitId AS {nameof(SaleDetailModel.ProductUnitId)},
                d.ProductUnit AS {nameof(SaleDetailModel.ProductUnit)},
                d.ProductUnitDiscount AS {nameof(SaleDetailModel.ProductUnitDiscount)},
                d.Quantity AS {nameof(SaleDetailModel.Quantity)},
                d.BatchNo AS {nameof(SaleDetailModel.BatchNo)},
                d.ExpiredDate AS {nameof(SaleDetailModel.ExpiredDate)},
                d.NetUnitCost AS {nameof(SaleDetailModel.NetUnitPrice)},
                d.DiscountAmount AS {nameof(SaleDetailModel.DiscountAmount)},
                d.TaxRate AS {nameof(SaleDetailModel.TaxRate)},
                d.TaxAmount AS {nameof(SaleDetailModel.TaxAmount)},
                d.TaxMethod AS {nameof(SaleDetailModel.TaxMethod)},
                d.TotalPrice AS {nameof(SaleDetailModel.TotalPrice)}
            FROM dbo.Sales s
            LEFT JOIN dbo.SaleDetails d ON s.Id = d.SaleId
            WHERE s.Id = @Id
        """;

        var saleDictionary = new Dictionary<Guid, UpsertSaleModel>();

        var saleModel = await connection.QueryAsync<UpsertSaleModel, SaleDetailModel, UpsertSaleModel>(
            sql,
            (sale, saleDetail) =>
            {
                if (!saleDictionary.TryGetValue(sale.Id, out var currentSale))
                {
                    currentSale = sale;
                    currentSale.SaleDetails = [];
                    saleDictionary.Add(currentSale.Id, currentSale);
                }

                if (saleDetail != null)
                {
                    currentSale.SaleDetails.Add(saleDetail);
                }

                return currentSale;
            },
            new { request.Id },
            splitOn: nameof(SaleDetailModel.Id)
        );

        var result = saleModel.FirstOrDefault();

        return result == null 
            ? Result.Failure<UpsertSaleModel>(Error.Failure(nameof(UpsertSaleModel), ErrorMessages.NotFound)) 
            : Result.Success(result);
    }
}

