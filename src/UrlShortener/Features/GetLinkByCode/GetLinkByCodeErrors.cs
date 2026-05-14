using UrlShortener.Results;

namespace UrlShortener.Features.GetLinkByCode;

public static class GetLinkByCodeErrors
{
    public static readonly Error NotFound =
        Error.NotFound("GetLinkByCode.NotFound", "No URL found for the provided code.");
}
