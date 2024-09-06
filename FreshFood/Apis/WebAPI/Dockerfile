#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FreshFood/Apis/WebAPI/WebAPI.csproj", "FreshFood/Apis/WebAPI/"]
COPY ["FreshFood/Services/CacheService/CacheService/CacheService.csproj", "FreshFood/Services/CacheService/CacheService/"]
COPY ["FreshFood/Services/CustomerService/CustomerService/CustomerService.csproj", "FreshFood/Services/CustomerService/CustomerService/"]
COPY ["FreshFood/Infrastructure/Database.Repositories/Database.Repositories.csproj", "FreshFood/Infrastructure/Database.Repositories/"]
COPY ["FreshFood/Shared/Models/Models.csproj", "FreshFood/Shared/Models/"]
COPY ["FreshFood/Infrastructure/Database/Database.csproj", "FreshFood/Infrastructure/Database/"]
COPY ["FreshFood/Infrastructure/Database.Entities/Database.Entities.csproj", "FreshFood/Infrastructure/Database.Entities/"]
COPY ["FreshFood/Shared/SecurityHelper/SecurityHelper.csproj", "FreshFood/Shared/SecurityHelper/"]
COPY ["FreshFood/Services/**/SecurityService.csproj", "FreshFood/Services/SecurityService/SecurityService/"]
RUN dotnet restore "./FreshFood/Apis/WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/FreshFood/Apis/WebAPI"
RUN dotnet build "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebAPI.csproj" --no-restore -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]