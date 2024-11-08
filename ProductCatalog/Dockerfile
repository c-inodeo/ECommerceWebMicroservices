FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY LocalNugetMicroservice /src/packages
COPY ProductCatalog/nuget.config /src

COPY ProductCatalog/ProductCatalog.csproj ./ProductCatalog/
RUN dotnet restore ./ProductCatalog/ProductCatalog.csproj --configfile /src/nuget.config

COPY . .

RUN dotnet build ProductCatalog/ProductCatalog.csproj -c Release -o /app/build
RUN dotnet publish ProductCatalog/ProductCatalog.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet ProductCatalog.dll && dotnet ef database update"]
