# Etapa 1: Compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar y restaurar
COPY *.csproj ./
RUN dotnet restore

# Copiar todo y compilar
COPY . ./
RUN dotnet publish -c Release -o /app

# Etapa 2: Ejecutar
FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MongoDocker.dll"]
