FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5000
EXPOSE 443
EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:80;http://+:5000;https://+:443;https://+:5001
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY ["Mega.WhatsAppApi.Api/Mega.WhatsAppApi.Api.csproj", "Mega.WhatsAppApi.Api/"]
RUN dotnet restore "Mega.WhatsAppApi.Api/Mega.WhatsAppApi.Api.csproj"
COPY . .

WORKDIR "/src/Mega.WhatsAppApi.Api"
RUN dotnet build -c Release -o /app/build

WORKDIR /src
COPY free.gsoftware.client.certificate.with.password.pfx free.gsoftware.client.certificate.with.password.pfx

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mega.WhatsAppApi.Api.dll"]
