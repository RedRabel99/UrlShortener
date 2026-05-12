# UrlShortener

Minimal-API URL shortener built on **ASP.NET Core (.NET 10)** with **EF Core 10 + PostgreSQL**. Short slugs are generated from a Postgres sequence and encoded with [Sqids](https://sqids.org/).

## Stack

- .NET 10 (`net10.0`) Minimal API
- EF Core 10 (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- PostgreSQL 16 (Docker Compose for local dev)
- Sqids for short-code encoding
