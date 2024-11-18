FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore ./OcelotGateway/OcelotGateway.csproj
RUN dotnet publish ./OcelotGateway/OcelotGateway.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 5000

ENTRYPOINT ["dotnet", "OcelotGateway.dll"]