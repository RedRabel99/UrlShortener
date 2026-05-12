namespace UrlShortener.Features.ShortenUrl;

public sealed record ShortenUrlResult(string Url, bool WasCreated);
