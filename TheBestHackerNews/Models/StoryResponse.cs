namespace TheBestHackerNews.Api.Models;

/// <summary>
/// Represents the API response for a best story.
/// </summary>
public sealed class StoryResponse
{
         
    /// <summary>
    /// Gets or sets the story title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the story URI.
    /// </summary>
    public string Uri { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the story author.
    /// </summary>
    public string PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the story publication time in UTC.
    /// </summary>
    public DateTimeOffset Time { get; set; }

    /// <summary>
    /// Gets or sets the story score.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Gets or sets the story comment count.
    /// </summary>
    public int CommentCount { get; set; }
}
