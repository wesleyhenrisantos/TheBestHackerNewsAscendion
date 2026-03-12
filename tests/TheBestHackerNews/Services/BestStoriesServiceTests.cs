using TheBestHackerNews.Api.Clients;
using TheBestHackerNews.Api.Models;
using TheBestHackerNews.Api.Options;
using TheBestHackerNews.Api.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;

namespace TheBestHackerNews.Tests.Services;

/// <summary>
/// Contains unit tests for <see cref="BestStoriesService"/>.
/// </summary>
public sealed class BestStoriesServiceTests
{
    private static BestStoriesService CreateService(Mock<IHackerNewsClient> clientMock, IMemoryCache? cache = null)
    {
        cache ??= new MemoryCache(new MemoryCacheOptions());
        var options = Options.Create(new HackerNewsOptions
        {
            MaxStoriesPerRequest = 50,
            FetchTopIdsBuffer = 50,
            BestStoriesCacheMinutes = 5,
            StoryCacheMinutes = 5
        });

        return new BestStoriesService(clientMock.Object, cache, options);
    }

    [Fact]
    public async Task GetBestStoriesAsync_ShouldReturnStoriesOrderedByScoreDescending()
    {
        var clientMock = new Mock<IHackerNewsClient>();
        clientMock.Setup(x => x.GetBestStoryIdsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([1, 2, 3]);

        clientMock.Setup(x => x.GetStoryAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HackerNewsStoryResponse { Id = 1, Type = "story", Title = "A", Url = "https://a", By = "alice", Time = 100, Score = 10, Descendants = 1 });
        clientMock.Setup(x => x.GetStoryAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HackerNewsStoryResponse { Id = 2, Type = "story", Title = "B", Url = "https://b", By = "bob", Time = 200, Score = 99, Descendants = 2 });
        clientMock.Setup(x => x.GetStoryAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HackerNewsStoryResponse { Id = 3, Type = "story", Title = "C", Url = "https://c", By = "carol", Time = 300, Score = 50, Descendants = 3 });

        var service = CreateService(clientMock);
        var result = await service.GetBestStoriesAsync(3, CancellationToken.None);

        Assert.Equal(["B", "C", "A"], result.Select(x => x.Title).ToArray());
    }

    [Fact]
    public async Task GetBestStoriesAsync_ShouldUseCacheAndAvoidDuplicatedRequests()
    {
        var clientMock = new Mock<IHackerNewsClient>();
        clientMock.Setup(x => x.GetBestStoryIdsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([1]);
        clientMock.Setup(x => x.GetStoryAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HackerNewsStoryResponse { Id = 1, Type = "story", Title = "A", Url = "https://a", By = "alice", Time = 100, Score = 10, Descendants = 1 });

        var cache = new MemoryCache(new MemoryCacheOptions());
        var service = CreateService(clientMock, cache);

        await service.GetBestStoriesAsync(1, CancellationToken.None);
        await service.GetBestStoriesAsync(1, CancellationToken.None);

        clientMock.Verify(x => x.GetBestStoryIdsAsync(It.IsAny<CancellationToken>()), Times.Once);
        clientMock.Verify(x => x.GetStoryAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetBestStoriesAsync_ShouldThrowForInvalidCount(int count)
    {
        var clientMock = new Mock<IHackerNewsClient>();
        var service = CreateService(clientMock);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetBestStoriesAsync(count, CancellationToken.None));
    }
}
