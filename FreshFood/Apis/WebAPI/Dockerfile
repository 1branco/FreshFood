#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Apis/WebAPI/WebAPI.csproj", "Apis/WebAPI/"]
COPY ["Services/CustomerService/CustomerService.csproj", "Services/CustomerService/"]
COPY ["Infrastructure/Database.Repositories/Database.Repositories.csproj", "Infrastructure/Database.Repositories/"]
COPY ["Shared/Models/Models.csproj", "Shared/Models/"]
COPY ["Infrastructure/Database/Database.csproj", "Infrastructure/Database/"]
COPY ["Infrastructure/Database.Entities/Database.Entities.csproj", "Infrastructure/Database.Entities/"]
COPY ["Shared/SecurityHelper/SecurityHelper.csproj", "Shared/SecurityHelper/"]
COPY ["Services/SecurityService/SecurityService.csproj", "Services/SecurityService/"]
COPY ["Services/CacheService/Cache/CacheService.csproj", "Services/CacheService/Cache/"]
RUN dotnet restore "./Apis/WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/Apis/WebAPI"
RUN dotnet build "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]