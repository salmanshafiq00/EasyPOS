using EasyPOS.Application.Common.Enums;
using EasyPOS.Domain.Trades;

namespace EasyPOS.Application.Features.Trades.Purchases.Shared;

internal static class PurchaseSharedService
{
    public static async Task<Guid?> GetPurchasePaymentId(ICommonQueryService commonQueryService, Purchase purchase)
    {
        var paymentStatuses = await commonQueryService.GetLookupDetailsAsync((int)LookupDevCode.PaymentStatus);

        if (purchase.GrandTotal == purchase.PaidAmount)
        {
            return paymentStatuses.FirstOrDefault(x => x.DevCode == (int)PaymentStatus.Paid)?.Id;
        }
        else if (purchase.GrandTotal > purchase.PaidAmount && purchase.PaidAmount > 0)
        {
            return paymentStatuses.FirstOrDefault(x => x.DevCode == (int)PaymentStatus.Partial)?.Id;
        }
        else
        {
            return paymentStatuses.FirstOrDefault(x => x.DevCode == (int)PaymentStatus.Due)?.Id;
        }
    }
}
