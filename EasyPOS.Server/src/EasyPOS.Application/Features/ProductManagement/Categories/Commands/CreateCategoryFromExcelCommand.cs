using ClosedXML.Excel;
using EasyPOS.Domain.Products;
using Microsoft.AspNetCore.Http;

namespace EasyPOS.Application.Features.ProductManagement.Categories.Commands;


public record CreateCategoryFromExcelCommand(
    IFormFile File) : ICacheInvalidatorCommand<int>
{
    public string CacheKey => CacheKeys.Category;
}

internal sealed class CreateCategoryFromExcelCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateCategoryFromExcelCommand, int>
{
    public async Task<Result<int>> Handle(CreateCategoryFromExcelCommand request, CancellationToken cancellationToken)
    {
        var items = await ProcessUploadFile(request.File);

        var entities = new List<Category>();

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

            entities.Add(new Category
            {
                Name = item.Name,
                Description = item.Description,
                ParentId = parentId,
            });

        }

        dbContext.Categories.AddRange(entities);
        var affectedRow = await dbContext.SaveChangesAsync(cancellationToken);

        return affectedRow;
    }

    private static async Task<List<CategoryExcelModel>> ProcessUploadFile(IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        using var wordbook = new XLWorkbook(stream);
        var worksheet = wordbook.Worksheet(1);
        var rows = worksheet.RangeUsed().RowsUsed();

        var lookupDetails = new List<CategoryExcelModel>();

        foreach (var row in rows.Skip(1))
        {

            lookupDetails.Add(new CategoryExcelModel
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

    private sealed class CategoryExcelModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public string? ParentName { get; set; }
        public string LookupName { get; set; }
    }
}
