#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Restaurants.API/Restaurants.API.csproj", "src/Restaurants.API/"]
COPY ["src/Restaurants.Application/Restaurants.Application.csproj", "src/Restaurants.Application/"]
COPY ["src/Restaurants.Domain/Restaurants.Domain.csproj", "src/Restaurants.Domain/"]
COPY ["src/Restaurants.Infrastructure/Restaurants.Infrastructure.csproj", "src/Restaurants.Infrastructure/"]
RUN dotnet restore "./src/Restaurants.API/Restaurants.API.csproj"
COPY . .
WORKDIR "/src/src/Restaurants.API"
RUN dotnet build "./Restaurants.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Restaurants.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Restaurants.API.dll"]