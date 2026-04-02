FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/AuthService/AuthService.csproj", "Services/AuthService/"]
COPY ["src/SharedContracts/SharedContracts.csproj", "SharedContracts/"]
RUN dotnet restore "Services/AuthService/AuthService.csproj"
COPY src/Services/AuthService/ Services/AuthService/
COPY src/SharedContracts/ SharedContracts/
RUN dotnet build "Services/AuthService/AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/AuthService/AuthService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.dll"]
