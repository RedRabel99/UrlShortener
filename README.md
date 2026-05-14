# UrlShortener

Minimal-API URL shortener built on **ASP.NET Core (.NET 10)** with **EF Core 10 + PostgreSQL**. Using CQRS and vertical slice. Short slugs are 7 characters long and generated from a Postgres sequence and encoded with [Sqids](https://sqids.org/).

## Stack

- .NET 10 (`net10.0`) Minimal API
- EF Core 10 (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- PostgreSQL 16 (Docker Compose for local dev)
- Sqids for short-code encoding

## Features

- `POST /shorten` — create short URL from long URL. Returns existing slug if long URL already shortened (idempotent).
- `GET /{code}` — resolve short code to long URL via HTTP redirect.

## Getting Started

### Prerequisites
- [Docker](https://www.docker.com/get-started) (Docker Compose scenario)

or

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet) (local dev scenario)

### Run with Docker Compose

**1. Create `.env` in repo root**

```env
SQIDS_ALPHABET=your-shuffled-alphabet-here
```

**2. Start the stack**

```bash
docker compose up --build
```

- `db` — PostgreSQL on port `5432`
- `api` — API on `http://localhost:8080`. Migrations apply on startup.

### Run Locally (without Docker)

**1. Start PostgreSQL**

```bash
docker compose up -d db
```

**2. Set Sqids alphabet user secret**

```bash
dotnet user-secrets set "Sqids:Alphabet" "your-shuffled-alphabet-here" --project src/UrlShortener
```

**3. Run the API**

```bash
dotnet run --project src/UrlShortener
```

Migrations apply on startup.

## Example Usage

```bash
# Shorten
curl -X POST http://localhost:8080/shorten \
  -H "Content-Type: application/json" \
  -d '{ "longUrl": "https://example.com/some/very/long/path" }'
# Response: { "shortUrl": "http://localhost:8080/Xy7aB2c" }

# Resolve (302 redirect)
curl -i http://localhost:8080/Xy7aB2c
```

## Roadmap

- Redis cache in front of `GET /{code}` to skip DB round-trip on hot slugs.
- Add custom slug support with validation and collision handling.
