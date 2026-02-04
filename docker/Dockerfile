# Use the official .NET 8 SDK image as base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Install dependencies if needed
RUN apt-get update && apt-get install -y \
    curl \
    wget \
    vim

# Set working directory
WORKDIR /app

# Copy project files
COPY . .

# Restore dependencies
WORKDIR /app/backend
RUN dotnet restore

# Build project
RUN dotnet build --configuration Release --no-restore

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Enable detailed error logging
ENV ASPNETCORE_DETAILEDERRORS=true
ENV ASPNETCORE_LOGLEVEL=Information

# Set entry point
ENTRYPOINT ["dotnet", "FakeQuotaSystem.dll"]
