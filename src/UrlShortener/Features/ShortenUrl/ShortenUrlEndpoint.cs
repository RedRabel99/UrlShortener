using UrlShortener.Abstractions.Endpoints;

namespace UrlShortener.Features.ShortenUrl;

public class ShortenUrlEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder) =>
        builder.MapPost("/shorten", async (
            ShortenUrlRequest request,
            ShortenUrlHandler handler,
            CancellationToken ct) =>
        {
            var cmd = new ShortenUrlCommand(request.LongUrl);

            var result = await handler.Handle(cmd, ct);
        });
}
