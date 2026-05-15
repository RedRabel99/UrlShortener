using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using UrlShortener.Abstractions.Messaging;
using UrlShortener.Infrastructure;
using UrlShortener.Results;

namespace UrlShortener.Features.GetLinkByCode;

public sealed class GetLinkByCodeQueryHandler(AppDbContext context, HybridCache cache) : IQueryHandler<GetLinkByCodeQuery, Result<GetLinkByCodeQueryResult>>
{
    private readonly AppDbContext _context = context;

    public async Task<Result<GetLinkByCodeQueryResult>> Handle(GetLinkByCodeQuery query, CancellationToken ct)
    {
        string? longUrl = await cache.GetOrCreateAsync(
            $"longurl:{query.Code}",
            async token =>
            {
                var result = await _context.Urls.AsNoTracking().FirstOrDefaultAsync(x => x.ShortUrl == query.Code, token);
                return result?.LongUrl;
            },
            cancellationToken: ct
            );
        
        if (longUrl is null)
        {
            return Result<GetLinkByCodeQueryResult>.Failure(GetLinkByCodeErrors.NotFound);
        }

        return Result<GetLinkByCodeQueryResult>.Success(new GetLinkByCodeQueryResult(longUrl));
    }
}
