# Usar a imagem do SDK .NET 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Usar a imagem do SDK .NET 8.0 para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar e restaurar dependências
COPY ["ICorteApi.csproj", "./"]
RUN dotnet restore "ICorteApi.csproj"

# Copiar todo o código e compilar
COPY . .
RUN dotnet build "ICorteApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar a aplicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ICorteApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Usar a imagem base para rodar a aplicação
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ICorteApi.dll"]
