using EasyPOS.Application.Features.Common.Queries;
using EasyPOS.Application.Features.Settings.CompanyInfos.Commands;
using EasyPOS.Application.Features.Settings.CompanyInfos.Queries;

namespace EasyPOS.WebApi.Endpoints.Settings;

public class CompanyInfos : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        //group.MapPost("GetAll", GetAll)
        //     .WithName("GetCompanyInfos")
        //     .Produces<PaginatedResponse<CompanyInfoModel>>(StatusCodes.Status200OK);

        group.MapGet("Get", Get)
             .WithName("GetCompanyInfo")
             .Produces<CompanyInfoModel>(StatusCodes.Status200OK);

        group.MapPost("Create", Create)
             .WithName("CreateCompanyInfo")
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("Update", Update)
             .WithName("UpdateCompanyInfo")
             .Produces(StatusCodes.Status200OK)
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound);

        //group.MapDelete("Delete/{id:Guid}", Delete)
        //     .WithName("DeleteCompanyInfo")
        //     .Produces(StatusCodes.Status204NoContent)
        //     .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        //group.MapPost("DeleteMultiple", DeleteMultiple)
        //     .WithName("DeleteCompanyInfos")
        //     .Produces(StatusCodes.Status204NoContent)
        //     .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private async Task<IResult> GetAll(ISender sender, GetCompanyInfoListQuery query)
    {
        var result = await sender.Send(query);
        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Get(ISender sender)
    {
        var result = await sender.Send(new GetCompanyInfoByIdQuery());

        var countrySelectList = await sender.Send(new GetSelectListQuery(
            Sql: SelectListSqls.LookupDetailNameKeySelectListByDevCodeSql,
            Parameters: new { DevCode = (int)LookupDevCode.Country },
            Key: $"{CacheKeys.LookupDetail}_{LookupDevCode.BarCodeSymbol}",
            AllowCacheList: true)
        );

        result.Value.OptionsDataSources.Add("countrySelectList", countrySelectList.Value);
        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> Create(ISender sender, [FromBody] CreateCompanyInfoCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
            onSuccess: () => Results.CreatedAtRoute("GetCompanyInfo", new { id = result.Value }),
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Update(ISender sender, [FromBody] UpdateCompanyInfoCommand command)
    {
        var result = await sender.Send(command);

        return result.Match(
            onSuccess: () => Results.Ok(),
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> Delete(ISender sender, Guid id)
    {
        var result = await sender.Send(new DeleteCompanyInfoCommand(id));

        return result.Match(
            onSuccess: Results.NoContent,
            onFailure: result.ToProblemDetails);
    }

    private async Task<IResult> DeleteMultiple(ISender sender, [FromBody] Guid[] ids)
    {
        var result = await sender.Send(new DeleteCompanyInfosCommand(ids));

        return result!.Match(
            onSuccess: Results.NoContent,
            onFailure: result!.ToProblemDetails);
    }
}
