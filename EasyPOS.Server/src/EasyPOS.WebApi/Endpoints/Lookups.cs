using EasyPOS.Application.Features.Admin.AppMenus.Queries;
using EasyPOS.Application.Features.Admin.AppNotifications.Queries;
using EasyPOS.Application.Features.Common.Queries;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Constants.CommonSqlConstants;
using EasyPOS.Application.Common.DapperQueries;
using EasyPOS.Application.Common.Extensions;
using EasyPOS.Application.Common.Models;
using EasyPOS.Application.Features.Lookups.Commands;
using EasyPOS.Application.Features.Lookups.Queries;
using EasyPOS.Domain.Shared;
using EasyPOS.Infrastructure.Communications;
using EasyPOS.WebApi.Extensions;
using EasyPOS.WebApi.Infrastructure;
using Microsoft.AspNetCore.SignalR;

namespace EasyPOS.WebApi.Endpoints;

public sealed class Lookups : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapPost("GetAll", GetAll)
             .WithName("GetLookups")
             .Produces<PaginatedResponse<LookupModel>>(StatusCodes.Status200OK);

        group.MapGet("Get/{id:Guid}", Get)
             .WithName("GetLookup")
             .Produces<LookupModel>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("Create", Create)
             .WithName("CreateLookup")
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("Update", Update)
             .WithName("UpdateLookup")
             .Produces(StatusCodes.Status200OK)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("Delete/{id:Guid}", Delete)
             .WithName("DeleteLookup")
             .Produces(StatusCodes.Status204NoContent)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private async Task<IResult> GetAll(
        ISender sender,
        IHubContext<NotificationHub, INotificationHub> context,
        IUser user,
        [FromBody] GetLookupListQuery query)
    {
        var result = await sender.Send(query);

        await context.Clients.All.ReceiveNotification(new AppNotificationModel
        {
            RecieverId = user.Id,
            SenderId = user.Id,
            Title = "Welcome to Lookup",
            Description = "Welcome to Lookup"
        });

        if (!query.IsInitialLoaded)
        {
            var parentSelectList = await sender.Send(new GetSelectListQuery(
                Sql: SelectListSqls.GetLookupParentSelectListSql,
                Parameters: new { },
                Key: CacheKeys.Lookup_All_SelectList,
                AllowCacheList: false)
            );

            result.Value.OptionsDataSources.Add("parentSelectList", parentSelectList.Value);
            result.Value.OptionsDataSources.Add("statusSelectList", UtilityExtensions.GetActiveInactiveSelectList());
        }

        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Get(ISender sender, Guid id)
    {
        var result = await sender.Send(new GetLookupByIdQuery(id));

        var parentSelectList = await sender.Send(new GetSelectListQuery(
            Sql: SelectListSqls.GetLookupSelectListSql,
            Parameters: new { },
            Key: CacheKeys.Lookup_All_SelectList,
            AllowCacheList: false)
        );

        result?.Value?.OptionsDataSources.Add("parentSelectList", parentSelectList.Value);
        result?.Value?.OptionsDataSources.Add("subjectSelectList", GetSubjectList());
        result?.Value?.OptionsDataSources.Add("subjectRadioSelectList", GetSubjectList());
        result?.Value?.OptionsDataSources.Add("multiParentSelectList", parentSelectList.Value);

        var appMenuTreeList = await sender.Send(new GetAppMenuTreeSelectList());
        result?.Value.OptionsDataSources.Add("appMenuTreeList", appMenuTreeList.Value);
        result?.Value.OptionsDataSources.Add("singleMenuTreeList", appMenuTreeList.Value);

        var parentTreeSelectList = await sender.Send(new GetAppMenuTreeSelectList());
        result?.Value?.OptionsDataSources.Add("treeSelectMenuseSelectList", parentTreeSelectList.Value);
        result?.Value?.OptionsDataSources.Add("treeSelectSingleMenuSelectList", parentTreeSelectList.Value);

        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Create(ISender sender, [FromBody] CreateLookupCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
            onSuccess: () => Results.CreatedAtRoute(nameof(Get), new { id = result.Value }),
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Update(ISender sender, [FromBody] UpdateLookupCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
            onSuccess: () => Results.Ok(),
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Delete(ISender sender, Guid id)
    {
        var result = await sender.Send(new DeleteLookupCommand(id));

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: result.ToProblemDetails);
    }

    private List<SelectListModel> GetSubjectList()
    {
        return new List<SelectListModel>
        {
            new SelectListModel{ Name = "Accounting", Id = "A" },
            new SelectListModel{ Name = "Marketing", Id = "M" },
            new SelectListModel{ Name = "Production", Id = "P" },
            new SelectListModel{ Name = "Research", Id = "R" }
        };
    }
}
