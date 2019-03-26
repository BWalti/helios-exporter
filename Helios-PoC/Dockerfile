FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY Helios-PoC/Helios-PoC.csproj Helios-PoC/
RUN dotnet restore Helios-PoC/Helios-PoC.csproj
COPY . .
WORKDIR /src/Helios-PoC
RUN dotnet build Helios-PoC.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Helios-PoC.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 9091
ENTRYPOINT ["dotnet", "Helios-PoC.dll"]
