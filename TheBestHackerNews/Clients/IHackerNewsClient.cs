using TheBestHackerNews.Api.Models;

namespace TheBestHackerNews.Api.Clients;

/// <summary>
/// Defines the contract used to access the Hacker News API.
/// </summary>
public interface IHackerNewsClient
{
    /// <summary>
    /// Retrieves the identifiers of the current best stories.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The story identifiers in the order returned by Hacker News.</returns>
    Task<IReadOnlyList<long>> GetBestStoryIdsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the details of a single story.
    /// </summary>
    /// <param name="storyId">The story identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The story details, or <c>null</c> if unavailable.</returns>
    Task<HackerNewsStoryResponse?> GetStoryAsync(long storyId, CancellationToken cancellationToken);
}
