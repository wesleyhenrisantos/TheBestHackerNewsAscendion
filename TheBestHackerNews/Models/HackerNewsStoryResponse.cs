using System.Text.Json.Serialization;

namespace TheBestHackerNews.Api.Models;

/// <summary>
/// Represents the raw story payload returned by the Hacker News API.
/// </summary>
public sealed class HackerNewsStoryResponse
{
    /// <summary>
    /// Gets or sets the story identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the item type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the story title.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the story URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the story author.
    /// </summary>
    [JsonPropertyName("by")]
    public string? By { get; set; }

    /// <summary>
    /// Gets or sets the story timestamp in Unix seconds.
    /// </summary>
    [JsonPropertyName("time")]
    public long Time { get; set; }

    /// <summary>
    /// Gets or sets the story score.
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; set; }

    /// <summary>
    /// Gets or sets the total number of descendants/comments.
    /// </summary>
    [JsonPropertyName("descendants")]
    public int Descendants { get; set; }
}
