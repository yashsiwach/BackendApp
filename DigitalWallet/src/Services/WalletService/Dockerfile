FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/WalletService/WalletService.csproj", "Services/WalletService/"]
COPY ["src/SharedContracts/SharedContracts.csproj", "SharedContracts/"]
RUN dotnet restore "Services/WalletService/WalletService.csproj"
COPY src/Services/WalletService/ Services/WalletService/
COPY src/SharedContracts/ SharedContracts/
RUN dotnet build "Services/WalletService/WalletService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/WalletService/WalletService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WalletService.dll"]
