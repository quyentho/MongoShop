FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app

COPY MongoShop/*.csproj MongoShop/MongoShop.csproj
COPY MongoShop.ElasticSearch.Indexer/*.csproj ./MongoShop.ElasticSearch.Indexer/MongoShop.ElasticSearch.Indexer.csproj
COPY MongoShop.PerformDBTask/*.csproj ./MongoShop.PerformDBTask/MongoShop.PerformDBTask.csproj
COPY MongoShop.Infrastructure/*.csproj ./MongoShop.Infrastructure/MongoShop.Infrastructure.csproj
COPY MongoShop.BusinessDomain/*.csproj ./MongoShop.BusinessDomain/MongoShop.BusinessDomain.csproj
COPY MongoShop.Server/*.csproj ./MongoShop.Server/MongoShop.Server.csproj
COPY MongoShop.UI/*.csproj ./MongoShop.UI/MongoShop.UI.csproj
RUN dotnet restore MongoShop/MongoShop.csproj
COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MongoShop.dll"]

