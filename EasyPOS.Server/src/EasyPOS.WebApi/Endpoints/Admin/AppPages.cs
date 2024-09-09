using EasyPOS.Application.Common.DapperQueries;
using EasyPOS.Application.Features.Admin.AppPages.Commands;
using EasyPOS.Application.Features.Admin.AppPages.Queries;
using EasyPOS.Domain.Shared;
using EasyPOS.WebApi.Extensions;
using EasyPOS.WebApi.Infrastructure;

namespace EasyPOS.WebApi.Endpoints.Admin;

public class AppPages : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapPost("GetAll", GetAll)
            .WithName("GetAppPages")
            .Produces<PaginatedResponse<AppPageModel>>(StatusCodes.Status200OK);

        group.MapGet("Get/{id}", Get)
            .WithName("GetAppPage")
            .Produces<AppPageModel>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("Create", Create)
            .WithName("CreateAppPage")
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("Update", Update)
            .WithName("UpdateAppPage")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("UpsertAppPage", UpsertAppPage)
            .WithName("UpsertAppPage")
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private async Task<IResult> GetAll(ISender sender, [FromBody] GetAppPageListQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Get(ISender sender, [FromRoute] Guid id)
    {
        var result = await sender.Send(new GetAppPageByIdQuery(id));

        return result.Match(
             onSuccess: () => Results.Ok(result.Value),
             onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Create(ISender sender, [FromBody] CreateAppPageCommand command)
    {
        var result = await sender.Send(command);

        return Results.Ok(result.Value);
    }

    private async Task<IResult> Update(ISender sender, [FromBody] UpdateAppPageCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
             onSuccess: () => Results.NoContent(),
             onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> UpsertAppPage(ISender sender, [FromBody] UpsertAppPageCommand command)
    {
        var result = await sender.Send(command);

        return Results.Ok(result);
    }
}
