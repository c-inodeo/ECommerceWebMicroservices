# Stage 1: Build the UserAuth project using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project file first to take advantage of Docker caching
COPY ProductCatalog/ProductCatalog.csproj ./ProductCatalog/
RUN dotnet restore ./ProductCatalog/ProductCatalog.csproj

# Now copy the rest of the source code
COPY . .

# Build and publish the project
WORKDIR /src/ProductCatalog
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Expose the port your application listens on (ensure it matches your K8S/service configuration)
EXPOSE 8080

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "ProductCatalog.dll"]
