# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
# COPY *.sln .
COPY ICorteApi/*.csproj ./ICorteApi/
RUN dotnet restore

# copy everything else and build app
COPY ICorteApi/. ./ICorteApi/
WORKDIR /source/ICorteApi
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ICorteApi.dll"]