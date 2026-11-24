FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Warehouse.Frontend.csproj .
RUN dotnet restore Warehouse.Frontend.csproj
COPY . .
RUN dotnet publish Warehouse.Frontend.csproj -c Release -o /out --no-restore

FROM nginx:stable-alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /out/wwwroot/ /usr/share/nginx/html/
