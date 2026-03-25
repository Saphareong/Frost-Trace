FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/FrostTrace.Api/FrostTrace.Api.csproj", "src/FrostTrace.Api/"]
RUN dotnet restore "src/FrostTrace.Api/FrostTrace.Api.csproj"
COPY . .
WORKDIR "/src/src/FrostTrace.Api"
RUN dotnet build "FrostTrace.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FrostTrace.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrostTrace.Api.dll"]
