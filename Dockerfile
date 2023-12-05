#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 1234

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Helios.Api/Helios.Api.csproj", "src/Helios.Api/"]
COPY ["src/Helios.Modbus/Helios.Modbus.csproj", "src/Helios.Modbus/"]
RUN dotnet restore "src/Helios.Api/Helios.Api.csproj"
COPY . .
WORKDIR "/src/src/Helios.Api"
RUN dotnet build "Helios.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Helios.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Helios.Api.dll"]