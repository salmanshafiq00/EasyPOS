using ClosedXML.Excel;
using EasyPOS.Application.Features.Customers.Queries;
using EasyPOS.Domain.Products;
using EasyPOS.Domain.Stakeholders;
using Microsoft.AspNetCore.Http;

namespace EasyPOS.Application.Features.Customers.Commands;


public record CreateCustomerFromExcelCommand(
    IFormFile File) : ICacheInvalidatorCommand<int>
{
    public string CacheKey => CacheKeys.Customer;
}

internal sealed class CreateCustomerFromExcelCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateCustomerFromExcelCommand, int>
{
    public async Task<Result<int>> Handle(CreateCustomerFromExcelCommand request, CancellationToken cancellationToken)
    {
        var items = await ProcessUploadFile(request.File);

        var entities = new List<Customer>();

        foreach (var item in items)
        {
            var countryId = await dbContext.Lookups
                .AsNoTracking()
                .Where(x => x.Name.ToLower() == item.CountryName.ToLower())
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            //if (countryId.IsNullOrEmpty()) continue;

            entities.Add(new Customer
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

        dbContext.Customers.AddRange(entities);
        var affectedRow = await dbContext.SaveChangesAsync(cancellationToken);

        return affectedRow;
    }

    private static async Task<List<CustomerModel>> ProcessUploadFile(IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        using var wordbook = new XLWorkbook(stream);
        var worksheet = wordbook.Worksheet(1);
        var rows = worksheet.RangeUsed().RowsUsed();

        var lookupDetails = new List<CustomerModel>();

        foreach (var row in rows.Skip(1))
        {

            lookupDetails.Add(new CustomerModel
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
