FROM mcr.microsoft.com/dotnet/aspnet:3.1-alpine AS base
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/MongoShop.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=quyen123
WORKDIR /app
EXPOSE 5000 5001 27017

FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS build
WORKDIR /src
COPY ./MongoShop/MongoShop/MongoShop.csproj MongoShop/
COPY ./MongoShop/MongoShop.BusinessDomain/MongoShop.BusinessDomain.csproj MongoShop.BusinessDomain/
COPY ./MongoShop/MongoShop.Infrastructure/MongoShop.Infrastructure.csproj MongoShop.Infrastructure/
COPY ./MongoShop/MongoShop.ElasticSearch.Indexer/MongoShop.ElasticSearch.Indexer.csproj MongoShop.ElasticSearch.Indexer/
RUN dotnet restore MongoShop/MongoShop.csproj

RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https -ep ./MongoShop/MongoShop.pfx -p quyen123
COPY ./MongoShop .

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /src/MongoShop/MongoShop.pfx ./MongoShop.pfx
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","MongoShop.dll", "watch", "run", "--no-restore", "--urls", "http://0.0.0.0:5000"]
