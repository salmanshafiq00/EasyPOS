# Stage 1: Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files separately for better caching
COPY ["src/EasyPOS.WebApi/EasyPOS.WebApi.csproj", "src/EasyPOS.WebApi/"]
COPY ["src/EasyPOS.Application/EasyPOS.Application.csproj", "src/EasyPOS.Application/"]
COPY ["src/EasyPOS.Domain/EasyPOS.Domain.csproj", "src/EasyPOS.Domain/"]
COPY ["src/EasyPOS.Infrastructure/EasyPOS.Infrastructure.csproj", "src/EasyPOS.Infrastructure/"]

# Restore dependencies only (caching layer)
RUN dotnet restore "./src/EasyPOS.WebApi/EasyPOS.WebApi.csproj"

# Copy the remaining source code
COPY . .

# Build the project
WORKDIR "/src/src/EasyPOS.WebApi"
RUN dotnet build "./EasyPOS.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EasyPOS.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Reduce the image size further by removing unnecessary files (e.g., XML docs, debug info)
RUN rm -rf /app/*.xml /app/*.pdb

ENTRYPOINT ["dotnet", "EasyPOS.WebApi.dll"]
