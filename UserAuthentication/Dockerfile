FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY LocalNugetMicroservice /src/packages
COPY UserAuthentication/nuget.config /src

COPY UserAuthentication/UserAuthentication.csproj ./UserAuthentication/
RUN dotnet restore ./UserAuthentication/UserAuthentication.csproj --configfile /src/nuget.config

COPY . .

RUN dotnet build UserAuthentication/UserAuthentication.csproj -c Release -o /app/build
RUN dotnet publish UserAuthentication/UserAuthentication.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "UserAuthentication.dll"]
