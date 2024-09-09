using System.Text.Json.Serialization;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Common.Extensions;
using EasyPOS.Application.Common.Security;
using EasyPOS.Domain.Shared;
using static EasyPOS.Application.Common.DapperQueries.SqlConstants;

namespace EasyPOS.Application.Features.Lookups.Queries;

[Authorize(Policy = Permissions.CommonSetup.Lookups.View)]
public record GetLookupByIdQuery(Guid? Id) : ICacheableQuery<LookupModel>
{
    [JsonIgnore]
    public string CacheKey => $"Lookup_{Id}";
    [JsonIgnore]
    public TimeSpan? Expiration => null;
    public bool? AllowCache => false;

}

internal sealed class GetLookupByIdQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetLookupByIdQuery, LookupModel>
{
    public async Task<Result<LookupModel>> Handle(GetLookupByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.Id.IsNullOrEmpty())
        {
            return new LookupModel()
            {
                Created = DateTime.Now,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(-1)),
                CreatedTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(15)),
                CreatedYear = DateTime.Now.Year,
                Subjects = ["A"],
                SubjectRadio = "A",
                Color = "#00ff62",
                DescEdit = "<h1>Write something.. </h1>",
                Menus = [Guid.Parse("728dfb56-e871-489b-3fe3-08dc90ab7866"), Guid.Parse("6bed6167-95fc-482c-3fe4-08dc90ab7866"), Guid.Parse("2901931b-4b76-453d-3fe5-08dc90ab7866")],
                SingleMenu = Guid.Parse("728dfb56-e871-489b-3fe3-08dc90ab7866"),
                TreeSelectMenus = [Guid.Parse("728dfb56-e871-489b-3fe3-08dc90ab7866"), Guid.Parse("6bed6167-95fc-482c-3fe4-08dc90ab7866"), Guid.Parse("2901931b-4b76-453d-3fe5-08dc90ab7866")],
                TreeSelectSingleMenu = Guid.Parse("728dfb56-e871-489b-3fe3-08dc90ab7866")

            };
        }
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT 
                L.Id AS {nameof(LookupModel.Id)}, 
                L.Name AS {nameof(LookupModel.Name)}, 
                L.Code {nameof(LookupModel.Code)}, 
                L.ParentId AS {nameof(LookupModel.ParentId)}, 
                L.Description AS {nameof(LookupModel.Description)},
                L.Status AS {nameof(LookupModel.Status)},
                --{S.CONV}(DATE, L.Created) AS {nameof(LookupModel.Created)},
                CAST(L.Created AS DATE) AS {nameof(LookupModel.CreatedDate)},
                CAST(L.Created AS TIME) AS {nameof(LookupModel.CreatedTime)},
                L.Created AS {nameof(LookupModel.Created)},
                YEAR(L.Created) AS {nameof(LookupModel.CreatedYear)}
            FROM dbo.Lookups AS l
            LEFT JOIN dbo.Lookups AS p ON p.Id = l.ParentId
            WHERE l.Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<LookupModel>(sql, new { query.Id });
    }
}
