# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
# Paths are now relative to the root where CafeManagement folder exists
COPY CafeManagement/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY CafeManagement/ ./
RUN dotnet publish -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Set environment variable for Render
ENV ASPNETCORE_URLS=http://+:10000

# Expose port 10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "CafeManagement.dll"]
