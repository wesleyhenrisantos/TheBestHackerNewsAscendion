using System.Net.Http.Json;
using TheBestHackerNews.Api.Models;

namespace TheBestHackerNews.Api.Clients;

/// <summary>
/// Provides a typed HTTP client for the Hacker News API.
/// </summary>
public sealed class HackerNewsClient(HttpClient httpClient) : IHackerNewsClient
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<long>> GetBestStoryIdsAsync(CancellationToken cancellationToken)
    {
        var ids = await httpClient.GetFromJsonAsync<List<long>>("beststories.json", cancellationToken);
        return ids ?? [];
    }

    /// <inheritdoc />
    public Task<HackerNewsStoryResponse?> GetStoryAsync(long storyId, CancellationToken cancellationToken)
    {
        return httpClient.GetFromJsonAsync<HackerNewsStoryResponse>($"item/{storyId}.json", cancellationToken);
    }
}
