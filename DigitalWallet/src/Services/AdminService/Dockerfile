FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/AdminService/AdminService.csproj", "Services/AdminService/"]
COPY ["src/SharedContracts/SharedContracts.csproj", "SharedContracts/"]
RUN dotnet restore "Services/AdminService/AdminService.csproj"
COPY src/Services/AdminService/ Services/AdminService/
COPY src/SharedContracts/ SharedContracts/
RUN dotnet build "Services/AdminService/AdminService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/AdminService/AdminService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminService.dll"]
