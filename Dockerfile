# Usar a imagem do SDK .NET 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Definir o diretório de trabalho
WORKDIR /src

# Copiar o arquivo .csproj e restaurar as dependências
COPY ["ICorteApi.csproj", "./"]
RUN dotnet restore "ICorteApi.csproj"

# Copiar o restante dos arquivos
COPY . .

# Compilar o projeto
RUN dotnet publish "ICorteApi.csproj" -c Release -o /app/publish

# Usar a imagem do runtime .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Definir o diretório de trabalho
WORKDIR /app

# Copiar os arquivos publicados do estágio anterior
COPY --from=build /app/publish .

# Definir o ponto de entrada
ENTRYPOINT ["dotnet", "ICorteApi.dll"]
