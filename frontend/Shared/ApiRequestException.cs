using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Warehouse.Frontend.Shared;

internal sealed class ApiRequestException(HttpStatusCode statusCode, string message)
    : HttpRequestException(message, null, statusCode)
{
    public static async Task EnsureSuccessAsync(HttpResponseMessage response, string fallback, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        if ((int)response.StatusCode is >= 400 and < 500)
        {
            try
            {
                var problem = await response.Content.ReadFromJsonAsync<ApiProblem>(cancellationToken);
                var messages = problem?.Errors?.Values.SelectMany(messages => messages).Distinct().ToArray();
                if (messages is { Length: > 0 })
                    throw new ApiRequestException(response.StatusCode, string.Join(" ", messages));

                if (response.StatusCode == HttpStatusCode.Conflict && !string.IsNullOrWhiteSpace(problem?.Title))
                    throw new ApiRequestException(response.StatusCode, $"{problem.Title} {problem.Detail}".TrimEnd());
            }
            catch (JsonException)
            {
            }
        }

        throw new ApiRequestException(response.StatusCode, fallback);
    }

    private sealed record ApiProblem(string? Title, string? Detail, Dictionary<string, string[]>? Errors);
}
