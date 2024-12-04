FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiază doar fișierul .csproj mai întâi
COPY party-helperbe/party-helperbe.csproj party-helperbe/

# Restaurează dependențele
RUN dotnet restore party-helperbe/party-helperbe.csproj

# Copiază restul fișierelor
COPY . .

WORKDIR /src/party-helperbe
RUN dotnet build party-helperbe.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish party-helperbe.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "party-helperbe.dll"]



