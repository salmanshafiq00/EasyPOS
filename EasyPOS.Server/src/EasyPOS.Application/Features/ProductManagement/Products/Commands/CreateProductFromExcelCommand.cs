using ClosedXML.Excel;
using EasyPOS.Domain.Products;
using Microsoft.AspNetCore.Http;

namespace EasyPOS.Application.Features.ProductManagement.Commands;

public record CreateProductFromExcelCommand(
    IFormFile File) : ICacheInvalidatorCommand<int>
{
    public string CacheKey => CacheKeys.Product;
}

internal sealed class CreateProductFromExcelCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateProductFromExcelCommand, int>
{
    public async Task<Result<int>> Handle(CreateProductFromExcelCommand request, CancellationToken cancellationToken)
    {
        var items = await ProcessUploadFile(request.File);

        var entities = new List<Product>();

        foreach (var item in items)
        {
            var lookupId = await dbContext.Lookups
                .AsNoTracking()
                .Where(x => x.Name.ToLower() == item.LookupName.ToLower())
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            if (lookupId.IsNullOrEmpty()) continue;

            Guid? parentId = null;
            if (!string.IsNullOrEmpty(item.ParentName))
            {
                parentId = await dbContext.Lookups
                    .AsNoTracking()
                    .Where(x => x.Name.ToLower() == item.ParentName.ToLower())
                    .Select(x => x.Id)
                    .SingleOrDefaultAsync();
            }

            entities.Add(new Product
            {
                Name = item.Name,
                Description = item.Description,
            });

        }

        dbContext.Products.AddRange(entities);
        var affectedRow = await dbContext.SaveChangesAsync(cancellationToken);

        return affectedRow;
    }

    private static async Task<List<ProductExcelModel>> ProcessUploadFile(IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        using var wordbook = new XLWorkbook(stream);
        var worksheet = wordbook.Worksheet(1);
        var rows = worksheet.RangeUsed().RowsUsed();

        var lookupDetails = new List<ProductExcelModel>();

        foreach (var row in rows.Skip(1))
        {

            lookupDetails.Add(new ProductExcelModel
            {
                Code = row.Cell(1).GetValue<string>(),
                Name = row.Cell(2).GetValue<string>(),
                ParentName = row.Cell(3).GetValue<string>(),
                LookupName = row.Cell(4).GetValue<string>(),
                Description = row.Cell(5).GetValue<string>(),
                Status = row.Cell(6).GetValue<string>()
            });
        }

        return lookupDetails;

    }

    private sealed class ProductExcelModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public string? ParentName { get; set; }
        public string LookupName { get; set; }
    }
}
