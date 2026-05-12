namespace UrlShortener.Features.ShortenUrl;

public sealed class ShortUrlOptions
{
    public const string SectionName = "ShortUrl";

    public required string BaseUrl { get; init; }
}
