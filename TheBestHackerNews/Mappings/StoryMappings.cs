using TheBestHackerNews.Api.Models;

namespace TheBestHackerNews.Api.Mappings;

/// <summary>
/// Provides mapping helpers for Hacker News story models.
/// </summary>
public static class StoryMappings
{
    /// <summary>
    /// Converts a raw Hacker News story response into the public API response model.
    /// </summary>
    /// <param name="source">The raw Hacker News story.</param>
    /// <returns>The mapped API response.</returns>
    public static StoryResponse ToResponse(this HackerNewsStoryResponse source)
    {
        return new StoryResponse
        {
            Title = source.Title ?? string.Empty,
            Uri = source.Url ?? string.Empty,
            PostedBy = source.By ?? string.Empty,
            Time = DateTimeOffset.FromUnixTimeSeconds(source.Time),
            Score = source.Score,
            CommentCount = source.Descendants
        };
    }
}
