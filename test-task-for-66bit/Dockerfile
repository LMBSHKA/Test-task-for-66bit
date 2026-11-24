FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY API/API.csproj API/
COPY Application/Application.csproj Application/
COPY Data/Data.csproj Data/
COPY Models/Models.csproj Models/
RUN dotnet restore API/API.csproj

COPY . .
RUN dotnet publish API/API.csproj -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /out/ .

EXPOSE 8080
ENTRYPOINT ["dotnet", "API.dll"]
