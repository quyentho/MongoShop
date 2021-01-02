using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Orders
{
    public class OrderServices : IOrderServices
    {
        private readonly IMongoCollection<Order> _collection;
        private readonly IDatabaseSetting _databaseSetting;
        private const string CollectionName = "order";
        private readonly IProductServices _productServices;


        public OrderServices(IDatabaseSetting databaseSetting, IProductServices productServices)
        {
            _databaseSetting = databaseSetting;
            this._productServices = productServices;
            var client = new MongoClient(_databaseSetting.ConnectionString);
            var database = client.GetDatabase(_databaseSetting.DatabaseName);

            _collection = database.GetCollection<Order>(CollectionName);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Order order)
        {
            await CheckProductQuantityInStockBeforeAdd(order);

            await ReduceProductQuantityInStockBasedOn(order);

            CreateNewInvoice(order);

            await _collection.InsertOneAsync(order);
        }

        private static void CreateNewInvoice(Order order)
        {
            Invoice newInvoice = new Invoice()
            {
                PaymentMethod = PaymentMethod.ShipCod,
                Status = InvoiceStatus.Pending
            };

            order.Invoice = newInvoice;
        }

        private async Task ReduceProductQuantityInStockBasedOn(Order order)
        {
            foreach (var orderedProduct in order.OrderedProducts)
            {
                var productFromDb = await _productServices.GetByIdAsync(orderedProduct.Product.Id);

                productFromDb.StockQuantity -= orderedProduct.OrderedQuantity;

                await _productServices.EditAsync(productFromDb.Id, productFromDb);
            }
        }

        // Throw exception if any product not have enough quantity in stock.
        private async Task CheckProductQuantityInStockBeforeAdd(Order order)
        {
            foreach (var orderedProduct in order.OrderedProducts)
            {
                var productFromDb = await _productServices.GetByIdAsync(orderedProduct.Product.Id);

                if (productFromDb.StockQuantity - orderedProduct.OrderedQuantity < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(orderedProduct.OrderedQuantity), "Not enough product in stock.");
                }

            }
        }

        /// <inheritdoc/>
        public Task DeleteAsync(string id, Order order)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task EditAsync(string id, Order order)
        {
            await _collection.ReplaceOneAsync(c => c.Id == id, order);
        }

        ///<inheritdoc/>
        public async Task<List<Order>> GetAllAsync()
        {
            var orders = await _collection.FindAsync(_ => true);
            return await orders.ToListAsync();
        }

        public async Task<List<Order>> GetOrdersWithUnpaidInvoiceAsync()
        {
            var orders = await _collection.AsQueryable()
                .Where(o => o.Invoice.Status.Equals(InvoiceStatus.Pending)).ToListAsync();

            return orders;
        }
    }
}
