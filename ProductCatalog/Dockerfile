FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ProductCatalog/ProductCatalog.csproj ./ProductCatalog/
RUN dotnet restore ./ProductCatalog/ProductCatalog.csproj

COPY . .

WORKDIR /src/ProductCatalog
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ProductCatalog.dll"]
