# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SetelaServerV3.1.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
# Render assigns a random port, .NET needs to listen on 0.0.0.0
ENV ASPNETCORE_URLS=http://+:10000 
EXPOSE 10000
ENTRYPOINT ["dotnet", "SetelaServerV3.1.dll"]
