FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/NotificationService/NotificationService.csproj", "Services/NotificationService/"]
COPY ["src/SharedContracts/SharedContracts.csproj", "SharedContracts/"]
RUN dotnet restore "Services/NotificationService/NotificationService.csproj"
COPY src/Services/NotificationService/ Services/NotificationService/
COPY src/SharedContracts/ SharedContracts/
RUN dotnet build "Services/NotificationService/NotificationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/NotificationService/NotificationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.dll"]
