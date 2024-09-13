namespace EasyPOS.Application.Features.Categories.Queries;

public record GetCategoryByIdQuery(Guid Id) : ICacheableQuery<CategoryModel>
{
    public string CacheKey => $"{CacheKeys.Category}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => true;
}

internal sealed class GetCategoryByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetCategoryByIdQuery, CategoryModel>
{
    public async Task<Result<CategoryModel>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new CategoryModel();
        }
        var connection = sqlConnectionFactory.GetOpenConnection();

        string sql = $"""
            SELECT
                c.Id AS {nameof(CategoryModel.Id)},
                c.Name AS {nameof(CategoryModel.Name)},
                c.Description AS {nameof(CategoryModel.Description)},
                c.ParentId AS {nameof(CategoryModel.ParentId)},
                c.PhotoUrl AS {nameof(CategoryModel.PhotoUrl)}
            FROM dbo.Categories c
            LEFT JOIN dbo.Categories cp ON cp.Id = c.ParentId
            WHERE c.Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<CategoryModel>(sql, new { request.Id });
    }
}
