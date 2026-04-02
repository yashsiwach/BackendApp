FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/RewardsService/RewardsService.csproj", "Services/RewardsService/"]
COPY ["src/SharedContracts/SharedContracts.csproj", "SharedContracts/"]
RUN dotnet restore "Services/RewardsService/RewardsService.csproj"
COPY src/Services/RewardsService/ Services/RewardsService/
COPY src/SharedContracts/ SharedContracts/
RUN dotnet build "Services/RewardsService/RewardsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/RewardsService/RewardsService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RewardsService.dll"]
