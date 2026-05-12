using UrlShortener.Results;

namespace UrlShortener.Features.ShortenUrl;

internal static class UrlErrors
{
    public static readonly Error Empty =
        Error.Validation("Url.Empty", "URL must not be empty.");

    public static readonly Error TooLong =
        Error.Validation("Url.TooLong", "URL exceeds the 2048 character maximum.");

    public static readonly Error Unparseable =
        Error.Validation("Url.Unparseable", "URL could not be parsed as an absolute URL.");

    public static readonly Error InvalidScheme =
        Error.Validation("Url.InvalidScheme", "Only http and https URLs are accepted.");
}
