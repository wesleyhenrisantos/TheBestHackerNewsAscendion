using TheBestHackerNews.Api.Clients;
using TheBestHackerNews.Api.Mappings;
using TheBestHackerNews.Api.Models;
using TheBestHackerNews.Api.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace TheBestHackerNews.Api.Services;

/// <summary>
/// Retrieves and caches best stories from Hacker News.
/// </summary>
public sealed class BestStoriesService(
    IHackerNewsClient hackerNewsClient,
    IMemoryCache cache,
    IOptions<HackerNewsOptions> options) : IBestStoriesService
{
    private readonly HackerNewsOptions _options = options.Value;
    private const string BestStoryIdsCacheKey = "hn:best-story-ids";

    /// <summary>
    /// Tasks to retrieve the best stories from Hacker News, with caching and validation of the requested count.
    /// </summary>
    /// <param name="count">The number of stories to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A list of best stories.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the requested count is invalid.</exception>
    public async Task<IReadOnlyList<StoryResponse>> GetBestStoriesAsync(int count, CancellationToken cancellationToken)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "The requested number of stories must be greater than zero.");
        }

        if (count > _options.MaxStoriesPerRequest)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"The requested number of stories cannot exceed {_options.MaxStoriesPerRequest}.");
        }

        var storyIds = await GetBestStoryIdsAsync(cancellationToken);

        if(storyIds.Count == 0)
        {
            throw new InvalidOperationException("No best stories are currently available.");
        }

        var idsToFetch = storyIds.Take(Math.Max(count, Math.Min(_options.FetchTopIdsBuffer, storyIds.Count))).ToArray();

        var storyTasks = idsToFetch.Select(id => GetStoryCachedAsync(id, cancellationToken));
        var fetchedStories = await Task.WhenAll(storyTasks);

        return fetchedStories
            .Where(static story => story is not null && string.Equals(story.Type, "story", StringComparison.OrdinalIgnoreCase))
            .Select(static story => story!.ToResponse())
            .OrderByDescending(static story => story.Score)
            .ThenBy(static story => story.Time)
            .Take(count)
            .ToArray();
    }

    /// <summary>
    /// Task to retrieve a specific story by its ID, with validation of the story ID and error handling for not found stories.
    /// </summary>
    /// <param name="storyId">The unique identifier of the story to retrieve. Must be a positive integer.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The story data if found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the story ID is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the story could not be found.</exception>
    public async Task<StoryResponse> GetStoryAsync(long storyId, CancellationToken cancellationToken)
    {
        if (storyId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(storyId), "The story ID must be greater than zero.");
        }
        var story = await hackerNewsClient.GetStoryAsync(storyId, cancellationToken);

        if (story == null)
            throw new InvalidOperationException("The story could not be found.");

        return story.ToResponse();
    }


    private async Task<IReadOnlyList<long>> GetBestStoryIdsAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGetValue(BestStoryIdsCacheKey, out IReadOnlyList<long>? ids) && ids is not null)
        {
            return ids;
        }

        ids = await hackerNewsClient.GetBestStoryIdsAsync(cancellationToken);
        cache.Set(BestStoryIdsCacheKey, ids, TimeSpan.FromMinutes(_options.BestStoriesCacheMinutes));
        return ids;
    }

    private async Task<HackerNewsStoryResponse?> GetStoryCachedAsync(long storyId, CancellationToken cancellationToken)
    {
        var cacheKey = $"hn:story:{storyId}";
        if (cache.TryGetValue(cacheKey, out HackerNewsStoryResponse? story))
        {
            return story;
        }

        story = await hackerNewsClient.GetStoryAsync(storyId, cancellationToken);
        cache.Set(cacheKey, story, TimeSpan.FromMinutes(_options.StoryCacheMinutes));
        return story;
    }


}
