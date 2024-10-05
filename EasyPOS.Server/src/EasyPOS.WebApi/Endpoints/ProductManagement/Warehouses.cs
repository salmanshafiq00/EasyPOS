using EasyPOS.Application.Features.ProductManagement.Warehouses.Commands;
using EasyPOS.Application.Features.ProductManagement.Warehouses.Queries;

namespace EasyPOS.WebApi.Endpoints.ProductManagement;

public class Warehouses : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapPost("GetAll", GetAll)
             .WithName("GetWarehouses")
             .Produces<PaginatedResponse<WarehouseModel>>(StatusCodes.Status200OK);

        group.MapGet("Get/{id:Guid}", Get)
             .WithName("GetWarehouse")
             .Produces<WarehouseModel>(StatusCodes.Status200OK);

        group.MapPost("Create", Create)
             .WithName("CreateWarehouse")
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("Update", Update)
             .WithName("UpdateWarehouse")
             .Produces(StatusCodes.Status200OK)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("Delete/{id:Guid}", Delete)
             .WithName("DeleteWarehouse")
             .Produces(StatusCodes.Status204NoContent)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("DeleteMultiple", DeleteMultiple)
             .WithName("DeleteWarehouses")
             .Produces(StatusCodes.Status204NoContent)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("Upload", Upload)
             .WithName("WarehouseUpload")
             .Produces<int>(StatusCodes.Status200OK)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private async Task<IResult> GetAll(ISender sender, GetWarehouseListQuery query)
    {
        var result = await sender.Send(query);
        if (!query.IsInitialLoaded)
        {
        }
        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Get(ISender sender, Guid id)
    {
        var result = await sender.Send(new GetWarehouseByIdQuery(id));

        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Create(ISender sender, [FromBody] CreateWarehouseCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
            onSuccess: () => Results.CreatedAtRoute("GetWarehouse", new { id = result.Value }),
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Update(ISender sender, [FromBody] UpdateWarehouseCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
            onSuccess: () => Results.Ok(),
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Delete(ISender sender, Guid id)
    {
        var result = await sender.Send(new DeleteWarehouseCommand(id));

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> DeleteMultiple(ISender sender, [FromBody] Guid[] ids)
    {
        Result? result = null;
        foreach (var id in ids)
        {
            result = await sender.Send(new DeleteWarehouseCommand(id));
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

        var result = await sender.Send(new CreateWarehouseFromExcelCommand(file));

        return result!.Match(
            onSuccess: () => Results.Ok(result.Value),
            onFailure: result!.ToProblemDetails);
    }
}
