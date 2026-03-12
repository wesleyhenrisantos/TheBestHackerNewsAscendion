using TheBestHackerNews.Api.Models;
using TheBestHackerNews.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace TheBestHackerNews.Api.Controllers;

/// <summary>
/// Exposes endpoints for best stories retrieved from Hacker News.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class BestStoriesController(IBestStoriesService bestStoriesService) : ControllerBase
{
    /// <summary>
    /// Returns the first N best stories from Hacker News, ordered by score descending.
    /// </summary>
    /// <param name="n">The amount of stories to return.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An array of best stories.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IReadOnlyList<StoryResponse>>> Get([FromQuery] int n = 10, CancellationToken cancellationToken = default)
    {
        var result = await bestStoriesService.GetBestStoriesAsync(n, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns the story with the specified ID from Hacker News, if it exists.
    /// </summary>
    /// <remarks>Returns a 200 OK response with the story data if successful. Returns a 400 Bad Request if the
    /// input is invalid, or a 503 Service Unavailable if the service is unavailable.</remarks>
    /// <param name="storyId">The unique identifier of the story to retrieve. Must be a positive integer.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An ActionResult containing the story data if found; otherwise, an appropriate error response.</returns>
    [HttpGet("{storyId:long}")]
    [ProducesResponseType(typeof(StoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<StoryResponse>> GetById(long storyId, CancellationToken cancellationToken = default)
    {
        var result = await bestStoriesService.GetStoryAsync(storyId, cancellationToken);
        return Ok(result);
    }
}

                                                                                                                                                                                                                            