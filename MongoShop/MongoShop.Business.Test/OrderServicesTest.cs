using System.Collections.Generic;
using MongoDB.Driver;
using MongoShop.BusinessDomain;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using Moq;
using Xunit;

namespace MongoShop.Business.Test
{
    public class OrderServicesTest
    {
        private readonly IMongoCollection<Order> _collection;
        private const string CollectionName = "order";
        public OrderServicesTest()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MongoShop");
            _collection = database.GetCollection<Order>(CollectionName);
        }
        [Fact]
        public void GetOrdersWithUnpaidInvoice_AllUnpaidOrders_ReturnsAll()
        {
            List<Order> fakeOrders = new List<Order>()
            {
                new Order(){ Invoice = new Invoice(){ Status = InvoiceStatus.Pending}},
                new Order(){ Invoice = new Invoice(){ Status = InvoiceStatus.Pending}},
                new Order(){ Invoice = new Invoice(){ Status = InvoiceStatus.Pending}},
                new Order(){ Invoice = new Invoice(){ Status = InvoiceStatus.Pending}}
            };

            _collection.InsertMany(fakeOrders);
        }
    }
}
