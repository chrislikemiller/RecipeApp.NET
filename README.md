# RecipeApp

A comprehensive recipe management platform built with .NET 9 and Clean Architecture principles, featuring user authentication, recipe sharing, social interactions, and advanced filtering capabilities.
Readme written by Claude himself. :)

## Getting Started

### Prerequisites
- .NET 9 SDK
- PostgreSQL or SQL Server
- Visual Studio 2022 or VS Code

### Configuration
1. Configure database connection in `appsettings.json`
2. Set JWT configuration (issuer, audience, secret key)
3. Configure CORS for frontend integration
4. Set up user secrets for sensitive configuration

### Development Features
- **Swagger UI**: Interactive API documentation
- **Dummy Data Seeding**: Automatic test data generation
- **CORS Configuration**: Angular development support
- **Request Logging**: Comprehensive request tracing
- **Error Handling**: Detailed error responses in development

## Features

### Core Recipe Management
- **Recipe CRUD Operations**: Create, read, update, and delete recipes with rich metadata
- **Advanced Filtering**: Search by title, difficulty level, tags, and author
- **Pagination Support**: Efficient data retrieval with configurable page sizes
- **Sorting Options**: Sort by date, rating, difficulty, or title with ascending/descending order
- **Recipe Favorites**: Users can favorite/unfavorite recipes for quick access

### User Management & Authentication
- **JWT Authentication**: Secure token-based authentication system
- **User Registration & Login**: Complete user account management
- **Identity Integration**: Built on ASP.NET Core Identity with custom user entities

### Social Features
- **User Following**: Follow/unfollow other recipe creators
- **Recipe Ratings**: 5-star rating system for recipe evaluation
- **Comments System**: Threaded comments with heart reactions
- **Community Engagement**: Social interactions between users

### Data Management
- **Rich Recipe Model**: Comprehensive recipe structure with ingredients, tags, and metadata
- **Relational Data**: Properly normalized database schema with Entity Framework Core
- **Multi-Database Support**: PostgreSQL and SQL Server compatibility
- **Dummy Data Seeding**: Development environment data initialization

## Architecture

### Clean Architecture Implementation
The application follows Clean Architecture principles with clear separation of concerns:

- **API Layer** (`RecipeApp.API`): Controllers, middleware, and HTTP concerns
- **Application Layer** (`RecipeApp.Application`): Business logic, services, and DTOs
- **Domain Layer** (`RecipeApp.Domain`): Core entities and business rules
- **Infrastructure Layer** (`RecipeApp.Infrastructure`): Data access and external services

## Tech Stack

### Core Framework
- **.NET 9**: Latest .NET framework with C# 13 features
- **ASP.NET Core**: Web API framework with OpenAPI support
- **Entity Framework Core 9**: ORM with PostgreSQL and SQL Server providers

### Key Libraries

#### Authentication & Security
- **Microsoft.AspNetCore.Identity**: User management and role-based security
- **JWT Bearer Authentication**: Stateless token-based authentication
- **Microsoft.IdentityModel.Tokens**: Token validation and security

#### Data & Mapping
- **Mapster**: High-performance object mapping
- **FluentValidation**: Fluent validation rules
- **Z.ExtensionMethods**: Utility extensions for enhanced productivity

#### Development & Documentation
- **Swashbuckle**: OpenAPI/Swagger documentation generation
- **Microsoft.AspNetCore.OpenApi**: Native OpenAPI support
- **Polly**: Resilience and fault-handling patterns

#### Testing Framework
- **NUnit**: Unit and integration testing framework
- **NSubstitute**: Mocking framework for unit tests
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing support
- **Entity Framework InMemory**: In-memory database for testing

### Database Support
- **PostgreSQL**: Primary production database (Npgsql provider)
- **SQL Server**: Alternative database option
- **SQLite**: Lightweight option for development/testing
- **InMemory**: Testing database provider

