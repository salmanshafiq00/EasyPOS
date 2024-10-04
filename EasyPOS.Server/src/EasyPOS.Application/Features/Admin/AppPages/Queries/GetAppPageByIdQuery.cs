namespace EasyPOS.Application.Features.Admin.AppPages.Queries;

//[Authorize(Policy = Permissions.Admin.AppPages.View)]
public record GetAppPageByIdQuery(Guid Id) : ICacheableQuery<AppPageModel?>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.AppPage}_{Id}";
    [JsonIgnore]
    public TimeSpan? Expiration => null;
    public bool? AllowCache => true;

}

internal sealed class GetAppPageByIdQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetAppPageByIdQuery, AppPageModel?>
{
    public async Task<Result<AppPageModel?>> Handle(GetAppPageByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.Id.IsNullOrEmpty())
        {
            return new AppPageModel();
        }

        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT 
                ap.Id AS {nameof(AppPageModel.Id)}, 
                ap.Title AS {nameof(AppPageModel.Title)}, 
                ap.SubTitle AS {nameof(AppPageModel.SubTitle)}, 
                ap.ComponentName AS {nameof(AppPageModel.ComponentName)}, 
                ap.AppPageLayout AS {nameof(AppPageModel.AppPageLayout)}
            FROM dbo.AppPages AS ap
            WHERE ap.Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<AppPageModel>(sql, new { query.Id });
    }
}
