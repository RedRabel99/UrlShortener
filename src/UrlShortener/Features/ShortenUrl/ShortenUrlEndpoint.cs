using UrlShortener.Abstractions.Endpoints;
using UrlShortener.Results;

namespace UrlShortener.Features.ShortenUrl;

public sealed class ShortenUrlEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/shorten", async Task<IResult> (
            ShortenUrlRequest request,
            ShortenUrlHandler handler,
            CancellationToken ct) =>
        {
            var result = await handler.Handle(new ShortenUrlCommand(request.LongUrl), ct);
            if (!result.IsSuccess)
            {
                return result.ToProblemDetails();
            }

            var body = new { shortUrl = result.Value.Url };
            return result.Value.WasCreated
                ? TypedResults.Created(result.Value.Url, body)
                : TypedResults.Ok(body);
        });
}
