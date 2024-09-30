# Base Stage: Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build Stage: Building and publishing the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["User.API/User.API.csproj", "User.API/"]
COPY ["BaseShare.Common/BaseShare.Common.csproj", "BaseShare.Common/"]
COPY ["User.Application/User.Application.csproj", "User.Application/"]
COPY ["User.Domain/User.Domain.csproj", "User.Domain/"]
COPY ["User.Infrastructure/User.Infrastructure.csproj", "User.Infrastructure/"]
RUN dotnet restore "./User.API/User.API.csproj"
COPY . .
WORKDIR "/src/User.API"
RUN dotnet build "./User.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage: Publish the service to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./User.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage: The production environment
FROM base AS final
WORKDIR /app

# Copy the published app from the publish stage
COPY --from=publish /app/publish .

# Set the entrypoint to run the application
ENTRYPOINT ["dotnet", "User.API.dll"]