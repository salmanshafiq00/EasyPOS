using EasyPOS.Application.Common.Enums;
using EasyPOS.Domain.Enums;

namespace EasyPOS.Application.Features.Trades.Sales.Queries;

public record GetSaleByIdQuery(Guid Id) : ICacheableQuery<SaleModel>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.Sale}_{Id}";
    [JsonIgnore]
    public TimeSpan? Expiration => null;
    public bool? AllowCache => false;
}

internal sealed class GetSaleByIdQueryHandler(ISqlConnectionFactory sqlConnection, ICommonQueryService commonQueryService)
    : IQueryHandler<GetSaleByIdQuery, SaleModel>
{
    public async Task<Result<SaleModel>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new SaleModel()
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
                -- SaleModel fields (master)
                s.Id AS {nameof(SaleModel.Id)},
                s.SaleDate AS {nameof(SaleModel.SaleDate)},
                s.ReferenceNo AS {nameof(SaleModel.ReferenceNo)},
                s.WarehouseId AS {nameof(SaleModel.WarehouseId)},
                s.CustomerId AS {nameof(SaleModel.CustomerId)},
                s.BillerId AS {nameof(SaleModel.BillerId)},
                s.AttachmentUrl AS {nameof(SaleModel.AttachmentUrl)},
                s.SaleStatusId AS {nameof(SaleModel.SaleStatusId)},
                s.PaymentStatusId AS {nameof(SaleModel.PaymentStatusId)},
                s.TaxRate AS {nameof(SaleModel.TaxRate)},
                s.TaxAmount AS {nameof(SaleModel.TaxAmount)},
                s.DiscountType AS {nameof(SaleModel.DiscountType)},
                s.DiscountRate AS {nameof(SaleModel.DiscountRate)},
                s.DiscountAmount AS {nameof(SaleModel.DiscountAmount)},
                s.ShippingCost AS {nameof(SaleModel.ShippingCost)},
                s.GrandTotal AS {nameof(SaleModel.GrandTotal)},
                s.SaleNote AS {nameof(SaleModel.SaleNote)},
                s.StaffNote AS {nameof(SaleModel.StaffNote)},

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
                d.NetUnitPrice AS {nameof(SaleDetailModel.NetUnitPrice)},
                d.DiscountAmount AS {nameof(SaleDetailModel.DiscountAmount)},
                d.Tax AS {nameof(SaleDetailModel.Tax)},
                d.TaxAmount AS {nameof(SaleDetailModel.TaxAmount)},
                d.TaxMethod AS {nameof(SaleDetailModel.TaxMethod)},
                d.TotalPrice AS {nameof(SaleDetailModel.TotalPrice)}
            FROM dbo.Sales s
            LEFT JOIN dbo.SaleDetails d ON s.Id = d.SaleId
            WHERE s.Id = @Id
        """;

        var saleDictionary = new Dictionary<Guid, SaleModel>();

        var saleModel = await connection.QueryAsync<SaleModel, SaleDetailModel, SaleModel>(
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
            ? Result.Failure<SaleModel>(Error.Failure(nameof(SaleModel), ErrorMessages.NotFound)) 
            : Result.Success(result);
    }
}

