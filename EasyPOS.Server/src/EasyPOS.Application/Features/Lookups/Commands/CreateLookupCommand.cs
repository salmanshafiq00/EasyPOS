using System.Text.Json.Serialization;
using EasyPOS.Domain.Common;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace EasyPOS.Application.Features.Lookups.Commands;

public record CreateLookupCommand(
    string Name,
    string Code,
    string Description,
    bool Status,
    DateOnly CreatedDate,
    TimeOnly CreatedTime,
    DateTime Created,
    int CreatedYear,
    string Color,
    List<string> Subjects,
    string SubjectRadio,
    IFormFile? UploadFile = null,
    Guid? ParentId = null) : ICacheInvalidatorCommand<Guid>
{
    [JsonIgnore]
    public string CacheKey => CacheKeys.Lookup;
}

internal sealed class CreateLookupQueryHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateLookupCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateLookupCommand request, CancellationToken cancellationToken)
    {
        TimeOnly a = request.CreatedTime;
        var entity = new Lookup
        {
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            Status = request.Status,
            ParentId = request.ParentId,
            Created = request.Created.ToLocalTime(),
        };

        dbContext.Lookups.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        //return Result.Success(CommonMessage.SAVED_SUCCESSFULLY);
        return Result.Success(entity.Id);
    }
}
