# Use the official .NET 8.0 SDK image as base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Install dependencies
RUN dotnet restore

# Copy project files
COPY . .

# Set working directory
WORKDIR /app

# Build project
RUN dotnet build --configuration Release --no-restore

# Expose port
EXPOSE 80

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__OracleConnection=Data Source=(DESCRIPTION=(ADDRESS_LIST=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521));User Id=SYSTEM;Password=your_password;Connection Pool Timeout=30;
ENV ConnectionStrings__MySqlConnection=Server=localhost;Database=fakequota;Uid=root;Pwd=your_password;

# Start application
ENTRYPOINT ["dotnet", "FakeQuotaSystem.dll"]
