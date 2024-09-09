using System.Text.Json;
using System.Text;

namespace EasyPOS.WebApi.Middlewares;

public class DateTimeAdjustmentMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DateTimeAdjustmentMiddleware> _logger;

    public DateTimeAdjustmentMiddleware(RequestDelegate next, ILogger<DateTimeAdjustmentMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
        {
            // Buffer the request body
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            // Check if the request body is JSON
            if (context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
            {
                var jsonObject = JsonDocument.Parse(body);
                var rootElement = jsonObject.RootElement.Clone();
                var modifiedJson = AdjustDateTimeFields(rootElement);

                var modifiedBody = new StringContent(modifiedJson, Encoding.UTF8, "application/json");
                context.Request.Body = await modifiedBody.ReadAsStreamAsync();
            }
        }

        await _next(context);
    }

    private string AdjustDateTimeFields(JsonElement rootElement)
    {
        var jsonObject = JsonDocument.Parse(rootElement.GetRawText());
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        AdjustDateTimeFieldsRecursive(jsonObject.RootElement, writer);

        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private void AdjustDateTimeFieldsRecursive(JsonElement element, Utf8JsonWriter writer)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var property in element.EnumerateObject())
                {
                    writer.WritePropertyName(property.Name);
                    AdjustDateTimeFieldsRecursive(property.Value, writer);
                }
                writer.WriteEndObject();
                break;

            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    AdjustDateTimeFieldsRecursive(item, writer);
                }
                writer.WriteEndArray();
                break;

            case JsonValueKind.String:
                string dateString = element.GetString();
                if (dateString.Contains("T") && DateTime.TryParse(dateString, out var dateTime))
                {

                    var adjustedDateTime = dateTime.AddHours(6);
                    writer.WriteStringValue(adjustedDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                }
                else
                {
                    writer.WriteStringValue(dateString);
                }
                break;

            default:
                element.WriteTo(writer);
                break;
        }
    }
}
