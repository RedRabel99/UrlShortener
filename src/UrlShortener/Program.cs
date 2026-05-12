using Microsoft.EntityFrameworkCore;
using Sqids;
using UrlShortener.Abstractions.Endpoints;
using UrlShortener.Features.ShortenUrl;
using UrlShortener.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.AddScoped<ShortenUrlHandler>();

builder.Services.Configure<ShortUrlOptions>(
    builder.Configuration.GetSection(ShortUrlOptions.SectionName));

var sqidsAlphabet = builder.Configuration["Sqids:Alphabet"]
    ?? throw new InvalidOperationException("No sqids alphabet provided");

builder.Services.AddSingleton(new SqidsEncoder<long>(new SqidsOptions
{
    Alphabet = sqidsAlphabet,
    MinLength = 7
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();
