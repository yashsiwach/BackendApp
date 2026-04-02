FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5006

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/SupportTicketService/SupportTicketService.csproj", "Services/SupportTicketService/"]
COPY ["src/SharedContracts/SharedContracts.csproj", "SharedContracts/"]
RUN dotnet restore "Services/SupportTicketService/SupportTicketService.csproj"
COPY src/Services/SupportTicketService/ Services/SupportTicketService/
COPY src/SharedContracts/ SharedContracts/
RUN dotnet build "Services/SupportTicketService/SupportTicketService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/SupportTicketService/SupportTicketService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SupportTicketService.dll"]
