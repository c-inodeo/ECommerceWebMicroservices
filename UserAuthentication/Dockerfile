FROM mcr.microsoft.com/dotnet/sdk:8.0 AS nuget-cache
WORKDIR /src

COPY UserAuthentication/UserAuthentication.csproj ./UserAuthentication/

RUN dotnet restore ./UserAuthentication/UserAuthentication.csproj --disable-parallel

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY --from=nuget-cache /root/.nuget /root/.nuget

COPY ../ ./

RUN dotnet restore --disable-parallel --verbosity diag

RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "UserAuthentication.dll"]
