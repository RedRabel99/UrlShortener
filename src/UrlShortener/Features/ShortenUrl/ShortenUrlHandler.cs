using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Sqids;
using UrlShortener.Abstractions.Messaging;
using UrlShortener.Domain;
using UrlShortener.Infrastructure;
using UrlShortener.Results;

namespace UrlShortener.Features.ShortenUrl;

public sealed class ShortenUrlHandler(
    AppDbContext context,
    SqidsEncoder<long> sqids,
    IOptions<ShortUrlOptions> options,
    ILogger<ShortenUrlHandler> logger)
    : ICommandHandler<ShortenUrlCommand, Result<ShortenUrlResult>>
{
    private readonly string _baseUrl = options.Value.BaseUrl.TrimEnd('/');

    public async Task<Result<ShortenUrlResult>> Handle(ShortenUrlCommand command, CancellationToken ct)
    {
        var validation = Validate(command.LongUrl);
        if (validation is not null)
        {
            return Result<ShortenUrlResult>.Failure(validation);
        }

        var existing = await context.Urls
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.LongUrl == command.LongUrl, ct);
        if (existing is not null)
        {
            return Result<ShortenUrlResult>.Success(
                new ShortenUrlResult(BuildUrl(existing.ShortUrl), WasCreated: false));
        }

        var sequenceValue = await context.Database
            .SqlQuery<long>($"SELECT nextval('url_short_seq') AS \"Value\"") //adding alias cause sqlquery expects column named value
            .SingleAsync(ct);
        var slug = sqids.Encode(sequenceValue);

        var entity = new Url
        {
            LongUrl = command.LongUrl,
            ShortUrl = slug
        };
        context.Urls.Add(entity);

        try
        {
            await context.SaveChangesAsync(ct);
            logger.LogInformation("Shortened {LongUrl} to {Slug}", command.LongUrl, slug);
            return Result<ShortenUrlResult>.Success(
                new ShortenUrlResult(BuildUrl(slug), WasCreated: true));
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            context.Entry(entity).State = EntityState.Detached;

            logger.LogWarning(
                "Concurrent shorten for {LongUrl}; returning the existing row", command.LongUrl);

            var existingUrl = await context.Urls
                .AsNoTracking()
                .SingleAsync(u => u.LongUrl == command.LongUrl, ct);
            return Result<ShortenUrlResult>.Success(
                new ShortenUrlResult(BuildUrl(existingUrl.ShortUrl), WasCreated: false));
        }
    }

    private static Error? Validate(string longUrl)
    {
        //use manual validation for now
        if (string.IsNullOrWhiteSpace(longUrl)) return ShortenUrlErrors.Empty;
        if (longUrl.Length > 2048) return ShortenUrlErrors.TooLong;
        if (!Uri.TryCreate(longUrl, UriKind.Absolute, out var uri)) return ShortenUrlErrors.Unparseable;
        if (uri.Scheme is not ("http" or "https")) return ShortenUrlErrors.InvalidScheme;
        return null;
    }

    private string BuildUrl(string slug) => $"{_baseUrl}/{slug}";
}
