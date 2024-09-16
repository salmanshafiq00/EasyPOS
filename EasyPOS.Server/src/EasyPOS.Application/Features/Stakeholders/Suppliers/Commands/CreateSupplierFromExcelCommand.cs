using ClosedXML.Excel;
using EasyPOS.Application.Features.Suppliers.Queries;
using EasyPOS.Domain.Stakeholders;
using Microsoft.AspNetCore.Http;

namespace EasyPOS.Application.Features.Suppliers.Commands;


public record CreateSupplierFromExcelCommand(
    IFormFile File) : ICacheInvalidatorCommand<int>
{
    public string CacheKey => CacheKeys.Supplier;
}

internal sealed class CreateSupplierFromExcelCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateSupplierFromExcelCommand, int>
{
    public async Task<Result<int>> Handle(CreateSupplierFromExcelCommand request, CancellationToken cancellationToken)
    {
        var items = await ProcessUploadFile(request.File);

        var entities = new List<Supplier>();

        foreach (var item in items)
        {
            var countryId = await dbContext.Lookups
                .AsNoTracking()
                .Where(x => x.Name.ToLower() == item.CountryName.ToLower())
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            //if (countryId.IsNullOrEmpty()) continue;

            entities.Add(new Supplier
            {
                Name = item.Name,
                Email = item.Email,
                PhoneNo = item.PhoneNo,
                Mobile = item.Mobile,
                CountryId = item.CountryId,
                City = item.City,
                Address = item.Address,
            });

        }

        dbContext.Suppliers.AddRange(entities);
        var affectedRow = await dbContext.SaveChangesAsync(cancellationToken);

        return affectedRow;
    }

    private static async Task<List<SupplierModel>> ProcessUploadFile(IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        using var wordbook = new XLWorkbook(stream);
        var worksheet = wordbook.Worksheet(1);
        var rows = worksheet.RangeUsed().RowsUsed();

        var lookupDetails = new List<SupplierModel>();

        foreach (var row in rows.Skip(1))
        {

            lookupDetails.Add(new SupplierModel
            {
                Name = row.Cell(2).GetValue<string>(),
                Email = row.Cell(3).GetValue<string>(),
                PhoneNo = row.Cell(4).GetValue<string>(),
                Mobile = row.Cell(5).GetValue<string>(),
                CountryName = row.Cell(6).GetValue<string>(),
                City = row.Cell(6).GetValue<string>(),
                Address = row.Cell(6).GetValue<string>()
            });
        }

        return lookupDetails;

    }
}
