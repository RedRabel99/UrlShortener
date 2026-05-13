using UrlShortener.Abstractions.Messaging;
using UrlShortener.Results;

namespace UrlShortener.Features.GetLinkByCode;

public sealed record GetLinkByCodeQuery(string Code) : IQuery<Result<GetLinkByCodeQueryResult>>;
