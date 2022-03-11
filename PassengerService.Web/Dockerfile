FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 1448
EXPOSE 1488

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PassengerService.Web.csproj", "PassengerService.Web/"]
RUN dotnet restore "PassengerService.Web/PassengerService.Web.csproj"
COPY . "PassengerService.Web"
WORKDIR "/src/PassengerService.Web"
RUN dotnet build "PassengerService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PassengerService.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PassengerService.Web.dll"]
