#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 81
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/App.Hosting/App.Hosting.csproj", "App.Hosting/"]
COPY ["src/App.Application/App.Application.csproj", "App.Application/"]
COPY ["src/App.Core/App.Core.csproj", "App.Core/"]
COPY ["src/App.Framwork/App.Framwork.csproj", "App.Framwork/"]
RUN dotnet restore "App.Hosting/App.Hosting.csproj"
COPY . .
WORKDIR "/src/src/App.Hosting"
RUN dotnet build "App.Hosting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.Hosting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.Hosting.dll"]