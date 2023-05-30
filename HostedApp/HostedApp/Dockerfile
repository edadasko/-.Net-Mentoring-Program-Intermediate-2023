#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HostedApp/HostedApp.csproj", "HostedApp/"]
RUN dotnet restore "HostedApp/HostedApp.csproj"
COPY . .
WORKDIR "/src/HostedApp"
RUN dotnet build "HostedApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HostedApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "HostedApp.dll"]