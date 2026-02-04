# Use the official .NET 8 SDK image as the base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Install the AWS CLI if needed
# RUN apt-get update && apt-get install -y --no-install-recommends awscli

# Set working directory
WORKDIR /app

# Copy project files
COPY . /app

# Restore dependencies
WORKDIR /app/backend
RUN dotnet restore

# Build the project
RUN dotnet build --configuration Release --no-restore

# Expose port
EXPOSE 80
EXPOSE 443

# Set the entry point
ENTRYPOINT ["dotnet", "FakeQuotaSystem.dll"]
