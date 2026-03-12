# HackerNews Best Stories API

A sample ASP.NET Core Web API implementation for the Santander coding test.

## What this project does

This API retrieves the first **N** "best stories" from the Hacker News API and returns them ordered by **score descending**, matching the requested response contract.

## Technical highlights

- ASP.NET Core Web API
- Typed `HttpClient` via `IHttpClientFactory`
- In-memory caching to reduce load on the Hacker News API
- Parallel story fetching with `Task.WhenAll`
- Swagger/OpenAPI documentation
- Global exception handling with Problem Details
- Unit tests with xUnit and Moq
- XML documentation comments (`summary`) in English

## Solution structure

```text
src/
  HackerNewsBestStories.Api/
tests/
  HackerNewsBestStories.Tests/
```

## Requirements

- .NET 10 SDK

## How to run

```bash
dotnet restore
dotnet build
dotnet run --project ./src/HackerNewsBestStories.Api/HackerNewsBestStories.Api.csproj
```

The API will start locally and Swagger will be available in development.

## Endpoint

```http
GET /api/beststories?n=10
GET /api/beststories/{storyId}
```

### Sample response

```json
[
  {
    "title": "Some story title",
    "uri": "https://example.com",
    "postedBy": "someuser",
    "time": "2026-03-12T10:00:00+00:00",
    "score": 999,
    "commentCount": 120
  }
]
```

## Configuration

The main options live in `appsettings.json`:

- `BaseUrl`: Hacker News API base URL
- `BestStoriesCacheMinutes`: cache duration for best story IDs
- `StoryCacheMinutes`: cache duration for individual stories
- `TimeoutSeconds`: HTTP timeout
- `MaxStoriesPerRequest`: upper bound for `n`
- `FetchTopIdsBuffer`: amount of IDs prefetched to improve score-based ordering

## Assumptions made

- The endpoint accepts a query string parameter named `n`.
- The service validates `n` and rejects values less than or equal to zero.
- A practical upper bound is enforced to avoid very expensive requests.
- Stories are cached for a short period to improve throughput and reduce external API pressure.
- Only items with `type = "story"` are returned.

## Why a buffer is used

The Hacker News `beststories` endpoint returns IDs in Hacker News order, while the coding test asks for the **first N best stories sorted by score descending**.

To satisfy both requirements without fetching every story in the full list, the service fetches the first chunk of IDs (at least `n`, up to a configurable buffer), then sorts the fetched items by score and returns the top `n`.

## Possible future improvements

- Add distributed caching with Redis
- Add API rate limiting
- Add integration tests with `WebApplicationFactory`
- Add structured logging and metrics
- Add Docker support
- Add ETag or response caching when appropriate

## Notes

This repository is intentionally compact so it is easy to review during a coding test, while still showing production-oriented practices.
