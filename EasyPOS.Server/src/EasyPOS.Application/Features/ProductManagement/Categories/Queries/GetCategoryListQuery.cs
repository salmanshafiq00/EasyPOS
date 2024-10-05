namespace EasyPOS.Application.Features.ProductManagement.Categories.Queries;

[Authorize(Policy = Permissions.Categories.View)]
public record GetCategoryListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<CategoryModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Category}l_{PageNumber}_{PageSize}";
}

internal sealed class GetCategoryListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetCategoryListQuery, PaginatedResponse<CategoryModel>>
{
    public async Task<Result<PaginatedResponse<CategoryModel>>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                c.Id AS {nameof(CategoryModel.Id)},
                c.Name AS {nameof(CategoryModel.Name)},
                c.Description AS {nameof(CategoryModel.Description)},
                c.ParentId AS {nameof(CategoryModel.ParentId)},
                cp.Name AS {nameof(CategoryModel.ParentCategory)},
                c.PhotoUrl AS {nameof(CategoryModel.PhotoUrl)}
            FROM dbo.Categories c
            LEFT JOIN dbo.Categories cp ON cp.Id = c.ParentId
            """;

        var sqlWithOrders = $"""
            {sql} 
            ORDER BY c.Name
            """;

        return await PaginatedResponse<CategoryModel>
            .CreateAsync(connection, sql, request);
    }
}
