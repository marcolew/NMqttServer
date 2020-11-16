#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 1883

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["NMqttServer/NMqttServer.csproj", "NMqttServer/"]
RUN dotnet restore "NMqttServer/NMqttServer.csproj"
COPY . .
WORKDIR "/src/NMqttServer"
RUN dotnet build "NMqttServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NMqttServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NMqttServer.dll"]