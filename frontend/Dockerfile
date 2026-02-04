# Multi-stage build for Blazor WebAssembly
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release --no-restore -o /app/publish

# Use runtime image
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# Copy custom nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port
EXPOSE 8080

# Start nginx
CMD ["nginx", "-g", "daemon off;"]
