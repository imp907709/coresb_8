# Stage 1: Build the solution
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
EXPOSE 5003/tcp

# ASPNETCORE_URLS=http://localhost:5003 ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5003 
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy the solution file and restore the dependencies
COPY CoreSB.sln .
COPY CoreSBServer/CoreSBServer.csproj CoreSBServer/
COPY CoreSBShared/CoreSBShared.csproj CoreSBShared/
COPY CoreSBBL/CoreSBBL.csproj CoreSBBL/
RUN dotnet restore

# Copy the remaining project files and build the solution
COPY . .
RUN dotnet build CoreSBServer/CoreSBServer.csproj  -c Release -o /app/build --no-restore

# Stage 2: Publish the projects
FROM build AS publish
RUN dotnet publish CoreSBServer/CoreSBServer.csproj -c Release -o /app/publish --no-restore

# Stage 3: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "CoreSBServer.dll"]
