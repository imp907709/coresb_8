# SOLID | Vertical slice | DDD | traditional ( layered/onion/clean)

A .NET 7.0 solution with hybrid architecture approach
1 traditional layered Slow changing infrastructure layer ( aka clean | layered | onion) with polyglot elements - multi-database support
and 
2 Vertical Slice aprroach for Domain layer - Stores, Services for domain business objects. All domain aggreages, events and naming asumed to go here 
3 with API layer above - where classic web api leaves

clean architecture with polyglot multi-database support. Entity Framework (SQL Server/PostgreSQL), MongoDB, and Elasticsearch integration with generic repository patterns.

## Architecture - 3 Layered
- **CoreSBServer**: ASP.NET Core Web API entry point
- **CoreSBBL**: Business logic layer with logging services
- **CoreSBShared**: Shared utilities and infrastructure abstractions

## Key Features
- **Multi-Database Support**: SQL Server, MongoDB, and Elasticsearch
- **Generic Repository Pattern**: Multiple implementations for different ID types
- **Cross-Database**: Persists data to any of three databases 
- **Docker Environment**: Complete containerized setup with all services
- **Clean Architecture**: Clear separation of concerns across layers

## Technology Stack
- .NET 7.0 with C# 11 features
- Entity Framework Core 7.0.5
- MongoDB Driver 2.19.2
- Elasticsearch 8.5.3 with NEST client
- Docker Compose for development

## API Endpoints
- `GET /test` - Health check
- `POST /AddToAll` - Logging endpoint (persists to all databases)
- `GET /WeatherForecast` - Sample data endpoint
