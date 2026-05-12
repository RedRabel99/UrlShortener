using UrlShortener.Abstractions.Messaging;
using UrlShortener.Domain;
using UrlShortener.Infrastructure;
using UrlShortener.Results;

namespace UrlShortener.Features.ShortenUrl;

public sealed class ShortenUrlHandler(AppDbContext context) : ICommandHandler<ShortenUrlCommand, Result>
{
    public async Task<Result> Handle(ShortenUrlCommand command, CancellationToken ct)
    {
        var url = new Url
        {
            ShortUrl = "short",
            LongUrl = command.LongUrl
        };

        await context.Urls.AddAsync(url, ct);
        await context.SaveChangesAsync(ct);
        return Result.Success();
    }
}