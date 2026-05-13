using Microsoft.EntityFrameworkCore;
using UrlShortener.Abstractions.Messaging;
using UrlShortener.Infrastructure;
using UrlShortener.Results;

namespace UrlShortener.Features.GetLinkByCode;

public sealed class GetLinkByCodeQueryHandler(AppDbContext context) : IQueryHandler<GetLinkByCodeQuery, Result<GetLinkByCodeQueryResult>>
{
    private readonly AppDbContext _context = context;
    2 
    public async Task<Result<GetLinkByCodeQueryResult>> Handle(GetLinkByCodeQuery query, CancellationToken cancellationToken)
    {
        var result = await _context.Urls.FirstOrDefaultAsync(x => x.ShortUrl == query.Code, cancellationToken);

        if(result is null)
        {
            return Result<GetLinkByCodeQueryResult>.Failure(GetLinkByCodeErrors.NotFound);
        }

        return Result<GetLinkByCodeQueryResult>.Success(new GetLinkByCodeQueryResult(LongUrl: result.LongUrl));
    }
}