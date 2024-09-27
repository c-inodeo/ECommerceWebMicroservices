# Use the .NET SDK to build the UserAuth project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ../ ./


COPY UserAuthentication/UserAuthentication.csproj UserAuthentication/
RUN dotnet restore

RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Use the .NET runtime to run the service
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserAuthentication.dll"]
