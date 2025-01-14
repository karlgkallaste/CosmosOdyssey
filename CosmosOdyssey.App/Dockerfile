# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files relative to the solution directory (context is set to `solution/`)
COPY ["CosmosOdyssey.App/CosmosOdyssey.App.csproj", "CosmosOdyssey.App/"]
COPY ["CosmosOdyssey.Services/CosmosOdyssey.Services.csproj", "CosmosOdyssey.Services/"]
COPY ["CosmosOdyssey.Domain/CosmosOdyssey.Domain.csproj", "CosmosOdyssey.Domain/"]
COPY ["CosmosOdyssey.Data/CosmosOdyssey.Data.csproj", "CosmosOdyssey.Data/"]

# Restore dependencies
RUN dotnet restore "CosmosOdyssey.App/CosmosOdyssey.App.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/CosmosOdyssey.App"
RUN dotnet build "CosmosOdyssey.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CosmosOdyssey.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CosmosOdyssey.App.dll"]
