#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/bm/handlers/BullMonitor.Analysis.Handlers/BullMonitor.Analysis.Handlers.csproj", "src/bm/handlers/BullMonitor.Analysis.Handlers/"]
COPY ["src/swe/SWE.Extensions/SWE.Extensions.csproj", "src/swe/SWE.Extensions/"]
COPY ["src/swe/SWE.Infrastructure.Abstractions/SWE.Infrastructure.Abstractions.csproj", "src/swe/SWE.Infrastructure.Abstractions/"]
COPY ["src/swe/SWE.Process/SWE.Process.csproj", "src/swe/SWE.Process/"]
COPY ["src/swe/SWE.Configuration/SWE.Configuration.csproj", "src/swe/SWE.Configuration/"]
COPY ["src/swe/SWE.Issue.Abstractions/SWE.Issue.Abstractions.csproj", "src/swe/SWE.Issue.Abstractions/"]
COPY ["src/swe/SWE.Rabbit.Abstractions/SWE.Rabbit.Abstractions.csproj", "src/swe/SWE.Rabbit.Abstractions/"]
COPY ["src/swe/SWE.Time/SWE.Time.csproj", "src/swe/SWE.Time/"]
COPY ["src/swe/SWE.Rabbit.Receiver/SWE.Rabbit.Receiver.csproj", "src/swe/SWE.Rabbit.Receiver/"]
COPY ["src/swe/SWE.Rabbit/SWE.Rabbit.csproj", "src/swe/SWE.Rabbit/"]
COPY ["src/bm/BullMonitor.Abstractions/BullMonitor.Abstractions.csproj", "src/bm/BullMonitor.Abstractions/"]
COPY ["src/bm/BullMonitor.Ticker.Process/BullMonitor.Ticker.Process.csproj", "src/bm/BullMonitor.Ticker.Process/"]
COPY ["src/swe/SWE.Infrastructure.Sql/SWE.Infrastructure.Sql.csproj", "src/swe/SWE.Infrastructure.Sql/"]
COPY ["src/bm/BullMonitor.Data.Sql/BullMonitor.Data.Sql.csproj", "src/bm/BullMonitor.Data.Sql/"]
COPY ["src/bm/BullMonitor.Data.Storage/BullMonitor.Data.Storage.csproj", "src/bm/BullMonitor.Data.Storage/"]
RUN dotnet restore "./src/bm/handlers/BullMonitor.Analysis.Handlers/./BullMonitor.Analysis.Handlers.csproj"
COPY . .
WORKDIR "/src/src/bm/handlers/BullMonitor.Analysis.Handlers"
RUN dotnet build "./BullMonitor.Analysis.Handlers.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BullMonitor.Analysis.Handlers.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BullMonitor.Analysis.Handlers.dll"]