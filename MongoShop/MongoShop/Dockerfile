#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MongoShop/MongoShop.csproj", "MongoShop/"]
COPY ["MongoShop.BusinessDomain/MongoShop.BusinessDomain.csproj", "MongoShop.BusinessDomain/"]

RUN dotnet restore "MongoShop/MongoShop.csproj"
COPY . .
WORKDIR "/src/MongoShop"
RUN dotnet build "MongoShop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MongoShop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MongoShop.dll"]