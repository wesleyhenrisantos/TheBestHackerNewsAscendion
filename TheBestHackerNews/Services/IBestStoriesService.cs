using TheBestHackerNews.Api.Models;

namespace TheBestHackerNews.Api.Services;

/// <summary>
/// Defines the contract for retrieving best stories.
/// </summary>
public interface IBestStoriesService
{
    /// <summary>
    /// Retrieves the first <paramref name="count"/> best stories and orders them by score descending.
    /// </summary>
    /// <param name="count">The amount of stories to return.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of best stories.</returns>
    Task<IReadOnlyList<StoryResponse>> GetBestStoriesAsync(int count, CancellationToken cancellationToken);


    /// <summary>
    /// Retrieves a single best story by its identifier.
    /// </summary>
    /// <param name="storyId">The story identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The best story.</returns>
    Task<StoryResponse> GetStoryAsync(long storyId, CancellationToken cancellationToken);
}
