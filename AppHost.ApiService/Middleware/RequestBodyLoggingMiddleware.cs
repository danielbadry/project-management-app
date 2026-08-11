using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AppHost.ApiService.Middleware;

public sealed class RequestBodyLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestBodyLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        if (request.ContentLength is > 0 || request.Headers.ContainsKey("Transfer-Encoding"))
        {
            request.EnableBuffering();

            using var reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync(context.RequestAborted);
            request.Body.Position = 0;

            logger.LogInformation(
                "HTTP request body {Method} {Path}: {RequestBody}",
                request.Method,
                request.Path,
                RedactSensitiveValues(body, request.ContentType));
        }
        else
        {
            logger.LogInformation(
                "HTTP request body {Method} {Path}: <empty>",
                request.Method,
                request.Path);
        }

        await next(context);
    }

    private static string RedactSensitiveValues(string body, string? contentType)
    {
        if (string.IsNullOrWhiteSpace(body) ||
            contentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) != true)
        {
            return body;
        }

        try
        {
            var json = JsonNode.Parse(body);
            RedactNode(json);
            return json?.ToJsonString() ?? body;
        }
        catch (JsonException)
        {
            return body;
        }
    }

    private static void RedactNode(JsonNode? node)
    {
        switch (node)
        {
            case JsonObject jsonObject:
                foreach (var property in jsonObject.ToList())
                {
                    RedactNode(property.Value);
                }

                break;

            case JsonArray jsonArray:
                foreach (var item in jsonArray)
                {
                    RedactNode(item);
                }

                break;
        }
    }
}
