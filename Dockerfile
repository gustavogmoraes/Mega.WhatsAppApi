FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY ["GS.ContainerManager.Api/GS.ContainerManager.Api.csproj", "GS.ContainerManager.Api/"]
RUN dotnet restore "GS.ContainerManager.Api/GS.ContainerManager.Api.csproj"
COPY . .

WORKDIR "/src/GS.ContainerManager.Api"
RUN dotnet build -c Release -o /app/build

WORKDIR /src

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GS.ContainerManager.Api.dll"]
