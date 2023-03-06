FROM mcr.microsoft.com/dotnet/core/sdk:2.2.402 AS build
COPY ./ /app
WORKDIR /app/src/GR.WebHosts/GR.Cms

RUN apt update && apt install -y git

ENV ASPNETCORE_ENVIRONMENT=Production

RUN dotnet restore
RUN dotnet build
RUN dotnet publish -c Release -o dist/

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
ENV ASPNETCORE_ENVIRONMENT=Production

WORKDIR /app
COPY --from=build /app/src/GR.WebHosts/GR.Cms/dist .
ENTRYPOINT ["dotnet", "GR.Cms.dll"]
