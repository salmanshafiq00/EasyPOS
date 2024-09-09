using EasyPOS.WebApi.Extensions;
using EasyPOS.WebApi.Infrastructure;

namespace EasyPOS.WebApi.Endpoints;

public class Images : EndpointGroupBase
{
    private const string BaseDirectory = "Resources/images";

    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapGet("{fileName}", GetImage)
             .WithName("GetImage")
             .AllowAnonymous()
             .Produces(StatusCodes.Status401Unauthorized)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

    }

    private async Task<IResult> GetImage(string fileName)
    {
        // Validate and sanitize the fileName input
        if (string.IsNullOrWhiteSpace(fileName) || fileName.Contains(".."))
        {
            return Results.BadRequest("Invalid file path.");
        }
        var filePath = Path.Combine(BaseDirectory, fileName);

        if (!File.Exists(filePath))
        {
            return Results.NotFound("File not found.");
        }
        try
        {
            var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
            var contentType = fileExtension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return Results.File(memory, contentType);
        }
        catch
        {
            // Log exception details if necessary
            return Results.Problem("An error occurred while processing your request.");
        }
    }

}
