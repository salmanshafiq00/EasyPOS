using EasyPOS.Application.Features.Common.Queries;
using EasyPOS.Application.Features.ProductManagement.Commands;
using EasyPOS.Application.Features.ProductManagement.Queries;

namespace EasyPOS.WebApi.Endpoints.ProductManagement;

public class Products : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapPost("GetAll", GetAll)
             .WithName("GetProducts")
             .Produces<PaginatedResponse<ProductModel>>(StatusCodes.Status200OK);

        group.MapGet("Get/{id:Guid}", Get)
             .WithName("GetProduct")
             .Produces<ProductModel>(StatusCodes.Status200OK);

        group.MapPost("Create", Create)
             .WithName("CreateProduct")
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("Update", Update)
             .WithName("UpdateProduct")
             .Produces(StatusCodes.Status200OK)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("Delete/{id:Guid}", Delete)
             .WithName("DeleteProduct")
             .Produces(StatusCodes.Status204NoContent)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("DeleteMultiple", DeleteMultiple)
             .WithName("DeleteProducts")
             .Produces(StatusCodes.Status204NoContent)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("Upload", Upload)
             .WithName("ProductUpload")
             .Produces<int>(StatusCodes.Status200OK)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("GetProductSelectList", GetProductSelectList)
             .WithName("GetProductSelectList")
             .Produces<List<ProductSelectListModel>>(StatusCodes.Status200OK);
    }

    private async Task<IResult> GetAll(ISender sender, GetProductListQuery query)
    {
        var result = await sender.Send(query);
        if (!query.IsInitialLoaded)
        {
            var categorySelectList = await sender.Send(new GetSelectListQuery(
                Sql: SelectListSqls.CategorySelectListSql,
                Parameters: new { },
                Key: CacheKeys.Product_All_SelectList,
                AllowCacheList: true)
            );

            result.Value.OptionsDataSources.Add("categorySelectList", categorySelectList.Value);
        }
        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Get(ISender sender, Guid id)
    {
        var result = await sender.Send(new GetProductByIdQuery(id));

        var productTypeSelectList = await sender.Send(new GetSelectListQuery(
            Sql: SelectListSqls.GetLookupDetailSelectListByDevCodeSql,
            Parameters: new { DevCode = LookupDevCode.ProductType },
            Key: $"{CacheKeys.LookupDetail}_{LookupDevCode.ProductType}",
            AllowCacheList: true)
        );

        var barcodeSymbolSelectList = await sender.Send(new GetSelectListQuery(
            Sql: SelectListSqls.GetLookupDetailSelectListByDevCodeSql,
            Parameters: new { DevCode = LookupDevCode.BarCodeSymbol },
            Key: $"{CacheKeys.LookupDetail}_{LookupDevCode.BarCodeSymbol}",
            AllowCacheList: true)
        );

        var brandSelectList = await sender.Send(new GetSelectListQuery(
           Sql: SelectListSqls.BrandsSelectListSql,
           Parameters: new { },
           Key: CacheKeys.Brand_All_SelectList,
           AllowCacheList: true)
        );

        var taxesSelectList = await sender.Send(new GetSelectListQuery(
           Sql: SelectListSqls.TaxesSelectListSql,
           Parameters: new { },
           Key: CacheKeys.Tax_All_SelectList,
           AllowCacheList: true)
        );

        var categorySelectList = await sender.Send(new GetSelectListQuery(
            Sql: SelectListSqls.CategorySelectListSql,
            Parameters: new { },
            Key: CacheKeys.Category_All_SelectList,
            AllowCacheList: true)
        );

        var productUnitSelectList = await sender.Send(new GetSelectListQuery(
          Sql: SelectListSqls.UnitsSelectListSql,
          Parameters: new { },
          Key: CacheKeys.Unit_All_SelectList,
          AllowCacheList: true)
       );

        result.Value.OptionsDataSources.Add("productTypeSelectList", productTypeSelectList.Value);
        result.Value.OptionsDataSources.Add("barcodeSymbolSelectList", barcodeSymbolSelectList.Value);
        result.Value.OptionsDataSources.Add("brandSelectList", brandSelectList.Value);
        result.Value.OptionsDataSources.Add("categorySelectList", categorySelectList.Value);
        result.Value.OptionsDataSources.Add("productUnitSelectList", productUnitSelectList.Value);
        result.Value.OptionsDataSources.Add("taxesSelectList", taxesSelectList.Value);

        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Create(ISender sender, [FromBody] CreateProductCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
            onSuccess: () => Results.CreatedAtRoute("GetProduct", new { id = result.Value }),
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Update(ISender sender, [FromBody] UpdateProductCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
            onSuccess: () => Results.Ok(),
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Delete(ISender sender, Guid id)
    {
        var result = await sender.Send(new DeleteProductCommand(id));

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> DeleteMultiple(ISender sender, [FromBody] Guid[] ids)
    {
        Result? result = null;
        foreach (var id in ids)
        {
            result = await sender.Send(new DeleteProductCommand(id));
        }
        return result!.Match(
            onSuccess: Results.NoContent,
            onFailure: result!.ToProblemDetails);
    }

    private async Task<IResult> Upload(ISender sender, IHttpContextAccessor contextAccessor)
    {
        var file = contextAccessor.HttpContext.Request.Form.Files[0];

        if (file == null || file.Length == 0)
        {
            return Results.BadRequest("No file uploaded.");
        }

        var result = await sender.Send(new CreateProductFromExcelCommand(file));

        return result!.Match(
            onSuccess: () => Results.Ok(result.Value),
            onFailure: result!.ToProblemDetails);
    }

    private async Task<IResult> GetProductSelectList(ISender sender, bool allowCache = true)
    {
        var result =await sender.Send(new GetProductSelectListQuery(
            AllowCacheList: allowCache)
        );

        return TypedResults.Ok(result.Value);
    }
}
