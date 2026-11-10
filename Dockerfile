FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY NeuroPath.API/NeuroPath.API.csproj NeuroPath.API/
COPY NeuroPath.Core/NeuroPath.Core.csproj NeuroPath.Core/
COPY NeuroPath.Data/NeuroPath.Data.csproj NeuroPath.Data/
RUN dotnet restore NeuroPath.API/NeuroPath.API.csproj

COPY NeuroPath.API/ NeuroPath.API/
COPY NeuroPath.Core/ NeuroPath.Core/
COPY NeuroPath.Data/ NeuroPath.Data/
RUN dotnet publish NeuroPath.API/NeuroPath.API.csproj --configuration Release --no-restore --output /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN mkdir -p /app/logs /app/wwwroot/uploads /home/app/.aspnet/DataProtection-Keys \
    && chown -R app:app /app/logs /app/wwwroot /home/app/.aspnet

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
USER app

ENTRYPOINT ["dotnet", "NeuroPath.API.dll"]
