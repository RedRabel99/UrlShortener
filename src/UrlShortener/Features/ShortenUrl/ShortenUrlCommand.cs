using UrlShortener.Abstractions.Messaging;
using UrlShortener.Results;

namespace UrlShortener.Features.ShortenUrl;

public sealed record ShortenUrlCommand(string LongUrl) : ICommand<Result<ShortenUrlResult>>;
