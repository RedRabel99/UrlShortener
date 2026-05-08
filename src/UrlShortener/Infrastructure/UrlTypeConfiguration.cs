using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain;

namespace UrlShortener.Infrastructure;

public class UrlTypeConfiguration : IEntityTypeConfiguration<Url>
{
    public void Configure(EntityTypeBuilder<Url> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ShortUrl).IsRequired().HasMaxLength(10);
        builder.HasIndex(x => x.ShortUrl).IsUnique();
        builder.Property(x => x.LongUrl).IsRequired().HasMaxLength(2048);
    }
}
