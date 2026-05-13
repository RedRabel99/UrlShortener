using UrlShortener.Abstractions.Endpoints;
using UrlShortener.Results;

namespace UrlShortener.Features.GetLinkByCode;

public sealed class GetLinkByCodeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/{code}", async (string code, GetLinkByCodeQueryHandler handler, CancellationToken ct) =>
        {
            var result = await handler.Handle(new GetLinkByCodeQuery(code), ct);
            return result.IsSuccess ? TypedResults.Redirect(result.Value.LongUrl) : result.ToProblemDetails();
        });
    }
}
