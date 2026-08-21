using System.Text;
using System.Text.Json;
using EmployeeAPI.Application.Services;

namespace EmployeeAPI.Middleware
{
    public class LanguageTranslationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITranslationService _translationService;

        public LanguageTranslationMiddleware(RequestDelegate next, ITranslationService translationService)
        {
            _next = next;
            _translationService = translationService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            await using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            try
            {
                await _next(context);

                memoryStream.Position = 0;
                var bodyText = await new StreamReader(memoryStream, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
                memoryStream.Position = 0;

                var languageCode = GetLanguageCode(context);
                var translatedBody = await TranslateResponseBodyAsync(bodyText, languageCode);

                context.Response.Body = originalBodyStream;
                context.Response.ContentLength = Encoding.UTF8.GetByteCount(translatedBody);
                await context.Response.WriteAsync(translatedBody);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private static string GetLanguageCode(HttpContext context)
        {
            var header = context.Request.Headers["Accept-Language"].ToString();
            if (string.IsNullOrWhiteSpace(header))
            {
                return "en";
            }

            return header.Split(',')[0].Trim();
        }

        private async Task<string> TranslateResponseBodyAsync(string bodyText, string languageCode)
        {
            if (string.IsNullOrWhiteSpace(bodyText) || bodyText.TrimStart().StartsWith("<", StringComparison.Ordinal))
            {
                return bodyText;
            }

            try
            {
                var jsonDocument = JsonDocument.Parse(bodyText);
                if (jsonDocument.RootElement.ValueKind != JsonValueKind.Object && jsonDocument.RootElement.ValueKind != JsonValueKind.Array)
                {
                    return bodyText;
                }

                var json = jsonDocument.RootElement.GetRawText();
                var translatedJson = await TranslateJsonAsync(json, languageCode);
                return translatedJson;
            }
            catch (JsonException)
            {
                return await _translationService.TranslateAsync(bodyText, languageCode);
            }
        }

        private async Task<string> TranslateJsonAsync(string json, string languageCode)
        {
            try
            {
                var payload = JsonSerializer.Deserialize<JsonElement>(json);
                if (payload.ValueKind == JsonValueKind.Object)
                {
                    var translatedObject = new Dictionary<string, object?>();
                    foreach (var property in payload.EnumerateObject())
                    {
                        translatedObject[property.Name] = await TranslateValueAsync(property.Value, languageCode);
                    }

                    return JsonSerializer.Serialize(translatedObject, new JsonSerializerOptions { WriteIndented = false });
                }

                if (payload.ValueKind == JsonValueKind.Array)
                {
                    var translatedArray = new List<object?>();
                    foreach (var item in payload.EnumerateArray())
                    {
                        translatedArray.Add(await TranslateValueAsync(item, languageCode));
                    }

                    return JsonSerializer.Serialize(translatedArray, new JsonSerializerOptions { WriteIndented = false });
                }
            }
            catch (Exception)
            {
                // Ignore and fall back to raw response.
            }

            return json;
        }

        private async Task<object?> TranslateValueAsync(JsonElement value, string languageCode)
        {
            return value.ValueKind switch
            {
                JsonValueKind.Object => await TranslateObjectAsync(value, languageCode),
                JsonValueKind.Array => await TranslateArrayAsync(value, languageCode),
                JsonValueKind.String => await _translationService.TranslateAsync(value.GetString() ?? string.Empty, languageCode),
                JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False or JsonValueKind.Null => value.GetRawText(),
                _ => value.GetRawText()
            };
        }

        private async Task<object?> TranslateObjectAsync(JsonElement value, string languageCode)
        {
            var translatedObject = new Dictionary<string, object?>();
            foreach (var property in value.EnumerateObject())
            {
                translatedObject[property.Name] = await TranslateValueAsync(property.Value, languageCode);
            }

            return translatedObject;
        }

        private async Task<object?> TranslateArrayAsync(JsonElement value, string languageCode)
        {
            var translatedArray = new List<object?>();
            foreach (var item in value.EnumerateArray())
            {
                translatedArray.Add(await TranslateValueAsync(item, languageCode));
            }

            return translatedArray;
        }
    }
}
