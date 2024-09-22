using ClosedXML.Excel;
using EasyPOS.Application.Features.Trades.Purchases.Queries;
using EasyPOS.Domain.Trades;
using Microsoft.AspNetCore.Http;

namespace EasyPOS.Application.Features.Trades.Purchases.Commands;


public record CreatePurchaseFromExcelCommand(
    IFormFile File) : ICacheInvalidatorCommand<int>
{
    public string CacheKey => CacheKeys.Purchase;
}

internal sealed class CreatePurchaseFromExcelCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreatePurchaseFromExcelCommand, int>
{
    public async Task<Result<int>> Handle(CreatePurchaseFromExcelCommand request, CancellationToken cancellationToken)
    {
        //var items = await ProcessUploadFile(request.File);

        var entities = new List<Purchase>();

        //foreach (var item in items)
        //{
        //    var countryId = await dbContext.Lookups
        //        .AsNoTracking()
        //        .Where(x => x.Name.ToLower() == item.CountryName.ToLower())
        //        .Select(x => x.Id)
        //        .SingleOrDefaultAsync();

            //if (countryId.IsNullOrEmpty()) continue;

            //entities.Add(new Purchase
            //{
            //    Name = item.Name,
            //    Email = item.Email,
            //    PhoneNo = item.PhoneNo,
            //    Mobile = item.Mobile,
            //    CountryId = item.CountryId,
            //    City = item.City,
            //    Address = item.Address,
            //});

        //}

        dbContext.Purchases.AddRange(entities);
        var affectedRow = await dbContext.SaveChangesAsync(cancellationToken);

        return affectedRow;
    }

    //private static async Task<List<PurchaseModel>> ProcessUploadFile(IFormFile file)
    //{
    //    using var stream = new MemoryStream();
    //    await file.CopyToAsync(stream);

    //    using var wordbook = new XLWorkbook(stream);
    //    var worksheet = wordbook.Worksheet(1);
    //    var rows = worksheet.RangeUsed().RowsUsed();

    //    var lookupDetails = new List<PurchaseModel>();

    //    foreach (var row in rows.Skip(1))
    //    {

    //        lookupDetails.Add(new PurchaseModel
    //        {
    //            Name = row.Cell(2).GetValue<string>(),
    //            Email = row.Cell(3).GetValue<string>(),
    //            PhoneNo = row.Cell(4).GetValue<string>(),
    //            Mobile = row.Cell(5).GetValue<string>(),
    //            CountryName = row.Cell(6).GetValue<string>(),
    //            City = row.Cell(6).GetValue<string>(),
    //            Address = row.Cell(6).GetValue<string>()
    //        });
    //    }

    //    return lookupDetails;

    //}
}
