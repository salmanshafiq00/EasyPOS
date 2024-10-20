using EasyPOS.Application.Features.Trades.PurchasePayments.Queries;

namespace EasyPOS.Application.Features.Trades.Purchases.Queries;

public record GetPurchaseDetailByIdQuery(Guid Id) : ICacheableQuery<PurchaseInfoModel>
{
    public string CacheKey => $"{CacheKeys.Purchase}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => false;
}

internal sealed class GetPurchaseDetailByIdQueryHandler(
    ISqlConnectionFactory sqlConnectionFactory,
    ICommonQueryService commonQueryService) 
    : IQueryHandler<GetPurchaseDetailByIdQuery, PurchaseInfoModel>
{
    public async Task<Result<PurchaseInfoModel>> Handle(GetPurchaseDetailByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            return Result.Failure<PurchaseInfoModel>(Error.Failure(nameof(PurchaseInfoModel), ErrorMessages.NotFound));
        }

        var connection = sqlConnectionFactory.GetOpenConnection();

        // SQL query to get both Purchase and PurchaseDetails with necessary fields
        var sql = $"""
            SELECT 
                t.Id AS {nameof(PurchaseInfoModel.Id)},
                t.PurchaseDate AS {nameof(PurchaseInfoModel.PurchaseDate)},
                t.ReferenceNo AS {nameof(PurchaseInfoModel.ReferenceNo)},
                t.WarehouseId AS {nameof(PurchaseInfoModel.WarehouseId)},
                t.SupplierId AS {nameof(PurchaseInfoModel.SupplierId)},
                t.PurchaseStatusId AS {nameof(PurchaseInfoModel.PurchaseStatusId)},
                t.AttachmentUrl AS {nameof(PurchaseInfoModel.AttachmentUrl)},
                t.SubTotal AS {nameof(PurchaseInfoModel.SubTotal)},
                t.TaxRate AS {nameof(PurchaseInfoModel.TaxRate)},
                t.TaxAmount AS {nameof(PurchaseInfoModel.TaxAmount)},
                t.DiscountType AS {nameof(PurchaseInfoModel.DiscountType)},
                t.DiscountRate AS {nameof(PurchaseInfoModel.DiscountRate)},
                t.DiscountAmount AS {nameof(PurchaseInfoModel.DiscountAmount)},
                t.ShippingCost AS {nameof(PurchaseInfoModel.ShippingCost)},
                t.GrandTotal AS {nameof(PurchaseInfoModel.GrandTotal)},
                t.Note AS {nameof(PurchaseInfoModel.Note)},

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
                pd.TotalPrice AS {nameof(PurchaseDetailModel.TotalPrice)},

                pp.Id AS {nameof(PurchasePaymentModel.Id)},
                pp.PurchaseId AS {nameof(PurchasePaymentModel.PurchaseId)},
                pp.PaymentDate AS {nameof(PurchasePaymentModel.PaymentDate)},
                pp.ReceivedAmount AS {nameof(PurchasePaymentModel.ReceivedAmount)},
                pp.PayingAmount AS {nameof(PurchasePaymentModel.PayingAmount)},
                pp.ChangeAmount AS {nameof(PurchasePaymentModel.ChangeAmount)},
                pp.PaymentType AS {nameof(PurchasePaymentModel.PaymentType)},
                pt.Name AS {nameof(PurchasePaymentModel.PaymentTypeName)},
                pp.Note AS {nameof(PurchasePaymentModel.Note)}

            FROM dbo.Purchases t
            LEFT JOIN dbo.PurchaseDetails pd ON pd.PurchaseId = t.Id
            LEFT JOIN dbo.PurchasePayments pp ON pp.PurchaseId = t.Id
            LEFT JOIN dbo.LookupDetails pt ON pt.Id = pp.PaymentType
            WHERE t.Id = @Id
            """;

        var purchaseDictionary = new Dictionary<Guid, PurchaseInfoModel>();
        var purchaseDetailDictionary = new Dictionary<Guid, PurchaseDetailModel>(); // Track details
        var purchasePaymentDictionary = new Dictionary<Guid, PurchasePaymentModel>(); // Track payments

        var result = await connection.QueryAsync<PurchaseInfoModel, PurchaseDetailModel, PurchasePaymentModel, PurchaseInfoModel>(
            sql,
            (purchase, detail, payments) =>
            {
                if (!purchaseDictionary.TryGetValue(purchase.Id, out var purchaseEntry))
                {
                    purchaseEntry = purchase;
                    purchaseEntry.PurchaseDetails = new List<PurchaseDetailModel>();
                    purchaseEntry.PaymentDetails = new List<PurchasePaymentModel>();
                    purchaseDictionary.Add(purchase.Id, purchaseEntry);
                }

                // Add distinct PurchaseDetails
                if (detail != null && !purchaseDetailDictionary.ContainsKey(detail.Id))
                {
                    purchaseEntry.PurchaseDetails.Add(detail);
                    purchaseDetailDictionary[detail.Id] = detail; // Keep track of added details
                }

                // Add distinct PurchasePayments
                if (payments != null && !purchasePaymentDictionary.ContainsKey(payments.Id))
                {
                    purchaseEntry.PaymentDetails.Add(payments);
                    purchasePaymentDictionary[payments.Id] = payments; // Keep track of added payments
                }

                return purchaseEntry;
            },
            new { request.Id },
            splitOn: "Id, Id, Id"
        );

        var purchase = purchaseDictionary.Values.FirstOrDefault();
        if(purchase is not null)
        {
            purchase.CompanyInfo = await commonQueryService.GetCompanyInfoAsync(cancellationToken);
            purchase.Supplier = await commonQueryService.GetSupplierDetail(purchase.SupplierId, cancellationToken);
            return purchase;
        }
        else
        {
            return Result.Failure<PurchaseInfoModel>(Error.Failure(nameof(PurchaseInfoModel), ErrorMessages.NotFound));
        }
    }
}

