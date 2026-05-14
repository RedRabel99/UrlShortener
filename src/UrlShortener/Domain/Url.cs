namespace UrlShortener.Domain;

public sealed class Url
{
    public Guid Id { get; set; }
    public required string LongUrl { get; set; }
    public required string ShortUrl { get; set; }
}
