# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Development stage for hot reload with dotnet watch
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS development
WORKDIR /src

COPY certificate.crt /usr/local/share/ca-certificates/certificate.crt
RUN update-ca-certificates

COPY ["RecipeApp.API/RecipeApp.API.csproj", "RecipeApp.API/"]
COPY ["RecipeApp.Application/RecipeApp.Application.csproj", "RecipeApp.Application/"]
COPY ["RecipeApp.Domain/RecipeApp.Domain.csproj", "RecipeApp.Domain/"]
COPY ["RecipeApp.Infrastructure/RecipeApp.Infrastructure.csproj", "RecipeApp.Infrastructure/"]
RUN dotnet restore "./RecipeApp.API/RecipeApp.API.csproj"

COPY . .
WORKDIR "/src/RecipeApp.API"

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "watch", "run"]

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY certificate.crt /usr/local/share/ca-certificates/certificate.crt
RUN update-ca-certificates

COPY ["RecipeApp.API/RecipeApp.API.csproj", "RecipeApp.API/"]
COPY ["RecipeApp.Application/RecipeApp.Application.csproj", "RecipeApp.Application/"]
COPY ["RecipeApp.Domain/RecipeApp.Domain.csproj", "RecipeApp.Domain/"]
COPY ["RecipeApp.Infrastructure/RecipeApp.Infrastructure.csproj", "RecipeApp.Infrastructure/"]
RUN dotnet restore "./RecipeApp.API/RecipeApp.API.csproj"
COPY . .
WORKDIR "/src/RecipeApp.API"
RUN dotnet build "./RecipeApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RecipeApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecipeApp.API.dll"]