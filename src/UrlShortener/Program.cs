using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Sqids;
using UrlShortener.Abstractions.Endpoints;
using UrlShortener.Features.GetLinkByCode;
using UrlShortener.Features.ShortenUrl;
using UrlShortener.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultDbConnection"));
});

builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.AddScoped<ShortenUrlHandler>();
builder.Services.AddScoped<GetLinkByCodeQueryHandler>();
builder.Services.Configure<ShortUrlOptions>(
    builder.Configuration.GetSection(ShortUrlOptions.SectionName));

var sqidsAlphabet = builder.Configuration["Sqids:Alphabet"]
    ?? throw new InvalidOperationException("No sqids alphabet provided");

builder.Services.AddSingleton(new SqidsEncoder<long>(new SqidsOptions
{
    Alphabet = sqidsAlphabet,
    MinLength = 7
}));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
});

builder.Services.AddHybridCache(options =>
{
    options.MaximumPayloadBytes = 1024 * 1024; // 1MB
    options.MaximumKeyLength = 1024;
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromHours(6), //total (L1 + L2) TTL
        LocalCacheExpiration = TimeSpan.FromMinutes(5)
    };
});

var app = builder.Build();

await app.ApplyMigrationsAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();
