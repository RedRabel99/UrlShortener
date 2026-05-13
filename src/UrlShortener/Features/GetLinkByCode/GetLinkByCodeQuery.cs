using UrlShortener.Abstractions.Messaging;
using UrlShortener.Results;

namespace UrlShortener.Features.GetLinkByCode;

public sealed record GetLinkByCodeQuery(string code) : IQuery<Result<GetLinkByCodeQueryResult>>;
