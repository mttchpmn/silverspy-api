FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Database.Migration/Database.Migration.csproj", "Database.Migration/"]
RUN dotnet restore "Database.Migration/Database.Migration.csproj"
COPY . .
WORKDIR "/src/Database.Migration"
RUN dotnet build "Database.Migration.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Database.Migration.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Database.Migration.dll"]
