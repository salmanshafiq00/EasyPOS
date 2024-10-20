using EasyPOS.Application.Features.Common.Queries;
using EasyPOS.Application.Features.ProductManagement.Queries;
using EasyPOS.Application.Features.Trades.Purchases.Commands;
using EasyPOS.Application.Features.Trades.Purchases.Queries;
using EasyPOS.Application.Features.UnitManagement.Queries;

namespace EasyPOS.WebApi.Endpoints;

public class Purchases : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapPost("GetAll", GetAll)
             .WithName("GetPurchases")
             .Produces<PaginatedResponse<PurchaseModel>>(StatusCodes.Status200OK);

        group.MapGet("Get/{id:Guid}", Get)
             .WithName("GetPurchase")
             .Produces<PurchaseModel>(StatusCodes.Status200OK);

        group.MapGet("GetDetail/{id:Guid}", GetDetail)
             .WithName("GetPurchaseDetail")
             .Produces<PurchaseInfoModel>(StatusCodes.Status200OK);


        group.MapPost("Create", Create)
             .WithName("CreatePurchase")
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("Update", Update)
             .WithName("UpdatePurchase")
             .Produces(StatusCodes.Status200OK)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("Delete/{id:Guid}", Delete)
             .WithName("DeletePurchase")
             .Produces(StatusCodes.Status204NoContent)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("DeleteMultiple", DeleteMultiple)
             .WithName("DeletePurchases")
             .Produces(StatusCodes.Status204NoContent)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("Upload", Upload)
             .WithName("PurchaseUpload")
             .Produces<int>(StatusCodes.Status200OK)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("DeletePurchaseDetail", DeletePurchaseDetail)
             .WithName("DeletePurchaseDetail")
             .Produces(StatusCodes.Status204NoContent)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private async Task<IResult> GetAll(ISender sender, GetPurchaseListQuery query)
    {
        var result = await sender.Send(query);
        if (!query.IsInitialLoaded)
        {
        }
        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Get(ISender sender, Guid id)
    {
        var result = await sender.Send(new GetPurchaseByIdQuery(id));

        var warehousesSelectList = await sender.Send(new GetSelectListQuery(
            Sql: SelectListSqls.WarehouseSelectListSql,
            Parameters: new { },
            Key: $"{CacheKeys.Warehouse_All_SelectList}",
            AllowCacheList: true)
        );

        var suppliersSelectList = await sender.Send(new GetSelectListQuery(
            Sql: SelectListSqls.GetSupplierSelectListSql,
            Parameters: new { },
            Key: $"{CacheKeys.Supplier_All_SelectList}",
            AllowCacheList: true)
        );

        var purchaseStatusSelectList = await sender.Send(new GetSelectListQuery(
            Sql: SelectListSqls.GetLookupDetailSelectListByDevCodeSql,
            Parameters: new { DevCode = LookupDevCode.PurchaseStatus },
            Key: $"{CacheKeys.LookupDetail}_{LookupDevCode.PurchaseStatus}",
            AllowCacheList: false)
        );

        var taxesSelectList = await sender.Send(new GetSelectListQuery(
           Sql: SelectListSqls.TaxesSelectListSql,
           Parameters: new { },
           Key: CacheKeys.Tax_All_SelectList,
           AllowCacheList: true)
        );

        var productsSelectList = await sender.Send(new GetProductSelectListQuery(
            AllowCacheList: false)
        );

        var productUnitSelectList = await sender.Send(new GetUnitSelectListQuery(
           AllowCacheList: false)
        );

        result.Value.OptionsDataSources.Add("suppliersSelectList", suppliersSelectList.Value);
        result.Value.OptionsDataSources.Add("warehousesSelectList", warehousesSelectList.Value);
        result.Value.OptionsDataSources.Add("purchaseStatusSelectList", purchaseStatusSelectList.Value);
        result.Value.OptionsDataSources.Add("productsSelectList", productsSelectList.Value);
        result.Value.OptionsDataSources.Add("taxesSelectList", taxesSelectList.Value);
        result.Value.OptionsDataSources.Add("productUnitSelectList", productUnitSelectList.Value);



        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> GetDetail(ISender sender, Guid id)
    {
        var result = await sender.Send(new GetPurchaseDetailByIdQuery(id));

        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Create(ISender sender, [FromBody] CreatePurchaseCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
            onSuccess: () => Results.CreatedAtRoute("GetPurchase", new { id = result.Value }),
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Update(ISender sender, [FromBody] UpdatePurchaseCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
            onSuccess: () => Results.Ok(),
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Delete(ISender sender, Guid id)
    {
        var result = await sender.Send(new DeletePurchaseCommand(id));

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> DeleteMultiple(ISender sender, [FromBody] Guid[] ids)
    {
        Result? result = null;
        foreach (var id in ids)
        {
            result = await sender.Send(new DeletePurchaseCommand(id));
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

        var result = await sender.Send(new CreatePurchaseFromExcelCommand(file));

        return result!.Match(
            onSuccess: () => Results.Ok(result.Value),
            onFailure: result!.ToProblemDetails);
    }

    private async Task<IResult> DeletePurchaseDetail(ISender sender, Guid id)
    {
        var result = await sender.Send(new DeletePurchaseDetailCommand(id));

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: result.ToProblemDetails);
    }
}
