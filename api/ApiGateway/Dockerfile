# =========================
# Build Stage
# =========================

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Set working directory
WORKDIR /src

# Copy project file
COPY *.csproj ./

# Restore dependencies
RUN dotnet restore

# Copy remaining files
COPY . .

# Publish application
RUN dotnet publish -c Release -o /app/publish


# =========================
# Runtime Stage
# =========================

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

# Set working directory
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose application port
EXPOSE 8080

# Configure ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080

# Start application
ENTRYPOINT ["dotnet", "ApiGateway.dll"]