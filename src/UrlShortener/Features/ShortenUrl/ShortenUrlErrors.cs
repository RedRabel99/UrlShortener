using UrlShortener.Results;

namespace UrlShortener.Features.ShortenUrl;

public static class ShortenUrlErrors
{
    public static readonly Error Empty =
        Error.Validation("ShortenUrl.Empty", "URL must not be empty.");

    public static readonly Error TooLong =
        Error.Validation("ShortenUrl.TooLong", "URL exceeds the 2048 character maximum.");

    public static readonly Error Unparseable =
        Error.Validation("ShortenUrl.Unparseable", "URL could not be parsed as an absolute URL.");

    public static readonly Error InvalidScheme =
        Error.Validation("ShortenUrl.InvalidScheme", "Only http and https URLs are accepted.");
}
