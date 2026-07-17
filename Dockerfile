FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["IndurFarmsAPI.csproj", "./"]

RUN dotnet restore "IndurFarmsAPI.csproj"

COPY . .

WORKDIR "/src"

RUN dotnet publish "IndurFarmsAPI.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "IndurFarmsAPI.dll"]