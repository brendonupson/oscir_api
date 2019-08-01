FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY OSCiR/OSCiR.csproj OSCiR/
COPY Application/ApplicationLayer.csproj Application/
COPY Domain/DomainLayer.csproj Domain/
COPY Data/DataLayer.csproj Data/
RUN dotnet restore OSCiR/OSCiR.csproj
COPY . .
WORKDIR /src/OSCiR
RUN dotnet build OSCiR.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish OSCiR.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OSCiR.dll"]
