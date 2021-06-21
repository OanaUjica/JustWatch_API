#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:5.0 AS build
WORKDIR /src
COPY *.sln .
COPY Lab1_.NET/*.csproj Lab1_.NET/
RUN dotnet restore
COPY . .

# testing
FROM build AS testing
WORKDIR /src/Lab1_.NET
RUN dotnet build

# publish
FROM build AS publish
WORKDIR /src/Lab1_.NET
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "Colors.API.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Lab1_.NET.dll