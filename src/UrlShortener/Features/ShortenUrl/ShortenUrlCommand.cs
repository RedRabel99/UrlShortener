using UrlShortener.Abstractions.Messaging;
using UrlShortener.Results;

namespace UrlShortener.Features.ShortenUrl;

public class ShortenUrlCommand : ICommand<Result>
{
    public string LongUrl;

    public ShortenUrlCommand(string longUrl)
    {
        LongUrl = longUrl;
    }
}