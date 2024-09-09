using EasyPOS.Application.Common.Models;
using EasyPOS.WebApi.Extensions;
using EasyPOS.WebApi.Infrastructure;

namespace EasyPOS.WebApi.Endpoints.Admin;

public class ManageFiles : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapPost("Upload", Upload)
            .WithName("Upload")
            .Produces<FileResponse[]>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("RemoveFile", RemoveFile)
            .WithName("RemoveFile")
            .Produces<FileResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

    }


    private async Task<IResult> Upload(IHttpContextAccessor context)
    {
        var files = context.HttpContext.Request.Form.Files;

        var location = context.HttpContext.Request.Form["location"];

        if (string.IsNullOrEmpty(location) || string.IsNullOrWhiteSpace(location))
        {
            location = "files";
        }

        if (files is not null && files.Count == 0)
        {
            return Results.BadRequest("Files not found");
        }

        var fileResponses = new List<FileResponse>();

        foreach (var file in files)
        {
            if (file is null || file.Length == 0)
            {
                return Results.BadRequest("File is empty");
            }

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", location!);

            Directory.CreateDirectory(folderPath);

            var fileNameWithExt = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(folderPath, fileNameWithExt);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            var relativePath = Path.Combine($"{Path.DirectorySeparatorChar}Resources", location!, fileNameWithExt).Replace(@"\", "/");

            fileResponses.Add(new FileResponse(relativePath));
        }

        return Results.Ok(fileResponses);

    }

    public async Task<IResult> RemoveFile([FromBody] RemoveFileRequest removeFileReq)
    {
        // Convert the relative path to an absolute path
        string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), removeFileReq.RelativePath.TrimStart(Path.DirectorySeparatorChar, '/'));

        if (File.Exists(absolutePath))
        {
            try
            {
                File.Delete(absolutePath);
                await Task.CompletedTask;
                return Results.Ok("File removed successfully.");
            }
            catch (Exception)
            {
                // Handle exceptions like access issues, etc.
                return Results.Problem(
                   statusCode: StatusCodes.Status500InternalServerError,
                   title: "Internal server error");

            }
        }
        else
        {
            return Results.NotFound("File not found.");
        }
    }
}
