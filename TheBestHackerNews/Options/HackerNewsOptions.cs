namespace TheBestHackerNews.Api.Options;

/// <summary>
/// Represents the configurable settings used to access and cache Hacker News data.
/// </summary>
public sealed class HackerNewsOptions
{

    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public const string SectionName = "HackerNews";

    /// <summary>
    /// Gets or sets the base URL for the Hacker News API.
    /// </summary>
    public string BaseUrl { get; set; } = "https://hacker-news.firebaseio.com/v0/";

    /// <summary>
    /// Gets or sets the number of minutes used to cache the best story identifiers.
    /// </summary>
    public int BestStoriesCacheMinutes { get; set; } = 5;

    /// <summary>
    /// Gets or sets the number of minutes used to cache individual story details.
    /// </summary>
    public int StoryCacheMinutes { get; set; } = 10;

    /// <summary>
    /// Gets or sets the HTTP timeout in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Gets or sets the maximum number of stories allowed per request.
    /// </summary>
    public int MaxStoriesPerRequest { get; set; } = 200;

    /// <summary>
    /// Gets or sets how many best-story IDs should be prefetched to improve scoring accuracy.
    /// </summary>
    public int FetchTopIdsBuffer { get; set; } = 250;
}
