namespace EasyPOS.Application.Features.Trades.Purchases.Queries;

public record GetPurchaseByIdQuery(Guid Id) : ICacheableQuery<PurchaseModel>
{
    public string CacheKey => $"{CacheKeys.Purchase}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => false;
}

internal sealed class GetPurchaseByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetPurchaseByIdQuery, PurchaseModel>
{
    public async Task<Result<PurchaseModel>> Handle(GetPurchaseByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new PurchaseModel();
        }

        var connection = sqlConnectionFactory.GetOpenConnection();

        // SQL query to get both Purchase and PurchaseDetails
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
                t.OrderTax AS {nameof(PurchaseModel.OrderTax)},
                t.OrderTaxAmount AS {nameof(PurchaseModel.OrderTaxAmount)},
                t.OrderDiscount AS {nameof(PurchaseModel.OrderDiscount)},
                t.ShippingCost AS {nameof(PurchaseModel.ShippingCost)},
                t.GrandTotal AS {nameof(PurchaseModel.GrandTotal)},
                t.Note AS {nameof(PurchaseModel.Note)},

                -- Child record: PurchaseDetails
                pd.Id AS {nameof(PurchaseDetailModel.Id)},
                pd.PurchaseId AS {nameof(PurchaseDetailModel.PurchaseId)},
                pd.ProductId AS {nameof(PurchaseDetailModel.ProductId)},
                pd.Quantity AS {nameof(PurchaseDetailModel.Quantity)},
                pd.BatchNo AS {nameof(PurchaseDetailModel.BatchNo)},
                pd.ExpiredDate AS {nameof(PurchaseDetailModel.ExpiredDate)},
                pd.NetUnitCost AS {nameof(PurchaseDetailModel.NetUnitCost)},
                pd.Tax AS {nameof(PurchaseDetailModel.Tax)},
                pd.TaxAmount AS {nameof(PurchaseDetailModel.TaxAmount)},
                pd.TaxMethod AS {nameof(PurchaseDetailModel.TaxMethod)},
                pd.DiscountAmount AS {nameof(PurchaseDetailModel.DiscountAmount)},
                pd.SubTotal AS {nameof(PurchaseDetailModel.SubTotal)},
                p.Code AS {nameof(PurchaseDetailModel.Code)},
                p.Name AS {nameof(PurchaseDetailModel.Name)}
            FROM dbo.Purchases t
            LEFT JOIN dbo.PurchaseDetails pd ON pd.PurchaseId = t.Id
            LEFT JOIN dbo.Products p ON p.Id = pd.ProductId
            WHERE t.Id = @Id
            """;

        var purchaseDictionary = new Dictionary<Guid, PurchaseModel>();

        // Using multi-mapping to map both PurchaseModel and PurchaseDetailModel
        var result = await connection.QueryAsync<PurchaseModel, PurchaseDetailModel, PurchaseModel>(
            sql,
            (purchase, detail) =>
            {
                if (!purchaseDictionary.TryGetValue(purchase.Id, out var purchaseEntry))
                {
                    purchaseEntry = purchase;
                    purchaseEntry.PurchaseDetails = new List<PurchaseDetailModel>();
                    purchaseDictionary.Add(purchase.Id, purchaseEntry);
                }

                if (detail != null)
                {
                    purchaseEntry.PurchaseDetails.Add(detail);
                }

                return purchaseEntry;
            },
            new { Id = request.Id },
            splitOn: "Id" // Specifies the column to split the result set between master and child
        );

        return purchaseDictionary.Values.FirstOrDefault();
    }
}
