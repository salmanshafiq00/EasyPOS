namespace EasyPOS.Application.Features.Trades.Purchases.Queries;

public record GetPurchaseByIdQuery(Guid Id) : ICacheableQuery<PurchaseModel>
{
    public string CacheKey => $"{CacheKeys.Purchase}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => false;
}

internal sealed class GetPurchaseByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) 
    : IQueryHandler<GetPurchaseByIdQuery, PurchaseModel>
{
    public async Task<Result<PurchaseModel>> Handle(GetPurchaseByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            return new PurchaseModel();
        }

        var connection = sqlConnectionFactory.GetOpenConnection();

        // SQL query to get both Purchase and PurchaseDetails with necessary fields
        var sql = $"""
            SELECT 
                t.Id AS {nameof(PurchaseModel.Id)},
                t.PurchaseDate AS {nameof(PurchaseModel.PurchaseDate)},
                t.ReferenceNo AS {nameof(PurchaseModel.ReferenceNo)},
                t.WarehouseId AS {nameof(PurchaseModel.WarehouseId)},
                t.SupplierId AS {nameof(PurchaseModel.SupplierId)},
                t.PurchaseStatusId AS {nameof(PurchaseModel.PurchaseStatusId)},
                t.AttachmentUrl AS {nameof(PurchaseModel.AttachmentUrl)},
                t.SubTotal AS {nameof(PurchaseModel.SubTotal)},
                t.TaxRate AS {nameof(PurchaseModel.TaxRate)},
                t.TaxAmount AS {nameof(PurchaseModel.TaxAmount)},
                t.DiscountType AS {nameof(PurchaseModel.DiscountType)},
                t.DiscountRate AS {nameof(PurchaseModel.DiscountRate)},
                t.DiscountAmount AS {nameof(PurchaseModel.DiscountAmount)},
                t.ShippingCost AS {nameof(PurchaseModel.ShippingCost)},
                t.GrandTotal AS {nameof(PurchaseModel.GrandTotal)},
                t.Note AS {nameof(PurchaseModel.Note)},

                -- PurchaseDetails
                pd.Id AS {nameof(PurchaseDetailModel.Id)},
                pd.PurchaseId AS {nameof(PurchaseDetailModel.PurchaseId)},
                pd.ProductId AS {nameof(PurchaseDetailModel.ProductId)},
                pd.ProductCode AS {nameof(PurchaseDetailModel.ProductCode)},
                pd.ProductName AS {nameof(PurchaseDetailModel.ProductName)},
                pd.ProductUnitCost AS {nameof(PurchaseDetailModel.ProductUnitCost)},
                pd.ProductUnitPrice AS {nameof(PurchaseDetailModel.ProductUnitPrice)},
                pd.ProductUnitId AS {nameof(PurchaseDetailModel.ProductUnitId)},
                pd.ProductUnitDiscount AS {nameof(PurchaseDetailModel.ProductUnitDiscount)},
                pd.Quantity AS {nameof(PurchaseDetailModel.Quantity)},
                pd.BatchNo AS {nameof(PurchaseDetailModel.BatchNo)},
                pd.ExpiredDate AS {nameof(PurchaseDetailModel.ExpiredDate)},
                pd.NetUnitCost AS {nameof(PurchaseDetailModel.NetUnitCost)},
                pd.DiscountType AS {nameof(PurchaseDetailModel.DiscountType)},
                pd.DiscountRate AS {nameof(PurchaseDetailModel.DiscountRate)},
                pd.DiscountAmount AS {nameof(PurchaseDetailModel.DiscountAmount)},
                pd.TaxRate AS {nameof(PurchaseDetailModel.TaxRate)},
                pd.TaxAmount AS {nameof(PurchaseDetailModel.TaxAmount)},
                pd.TaxMethod AS {nameof(PurchaseDetailModel.TaxMethod)},
                pd.TotalPrice AS {nameof(PurchaseDetailModel.TotalPrice)}
            FROM dbo.Purchases t
            LEFT JOIN dbo.PurchaseDetails pd ON pd.PurchaseId = t.Id
            WHERE t.Id = @Id
            """;

        var purchaseDictionary = new Dictionary<Guid, PurchaseModel>();

        var result = await connection.QueryAsync<PurchaseModel, PurchaseDetailModel, PurchaseModel>(
            sql,
            (purchase, detail) =>
            {
                if (!purchaseDictionary.TryGetValue(purchase.Id, out var purchaseEntry))
                {
                    purchaseEntry = purchase;
                    purchaseEntry.PurchaseDetails = [];
                    purchaseDictionary.Add(purchase.Id, purchaseEntry);
                }

                if (detail != null)
                {
                    purchaseEntry.PurchaseDetails.Add(detail);
                }

                return purchaseEntry;
            },
            new { request.Id },
            splitOn: "Id"
        );

        var purchase = purchaseDictionary.Values.FirstOrDefault();
        return purchase != null ? Result.Success(purchase) : Result.Failure<PurchaseModel>(Error.Failure(nameof(PurchaseModel),ErrorMessages.NotFound));
    }
}

