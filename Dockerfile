# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and all project files
COPY ["ConductorSizing.sln", "./"]
COPY ["src/ConductorSizing.Domain/ConductorSizing.Domain.csproj", "src/ConductorSizing.Domain/"]
COPY ["src/ConductorSizing.Application/ConductorSizing.Application.csproj", "src/ConductorSizing.Application/"]
COPY ["src/ConductorSizing.Infrastructure/ConductorSizing.Infrastructure.csproj", "src/ConductorSizing.Infrastructure/"]
COPY ["src/ConductorSizing.Web/ConductorSizing.Web.csproj", "src/ConductorSizing.Web/"]

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build and publish
RUN dotnet publish src/ConductorSizing.Web/ConductorSizing.Web.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install dependencies for QuestPDF
RUN apt-get update && apt-get install -y \
    libgdiplus \
    && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Start the application
ENTRYPOINT ["dotnet", "ConductorSizing.Web.dll"]
