﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoShop.BusinessDomain.Products;
using MongoShop.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Orders
{
    public class OrderServices : IOrderServices
    {
        private readonly IMongoCollection<Order> _collection;
        private readonly IProductServices _productServices;
        private readonly string _collectionName;

        public OrderServices(IMongoClient mongoClient, IOptions<DatabaseSetting> settings, IProductServices productServices)
        {

            _collectionName = MongoDbHelper.GetCollectionName(this.GetType().Name);

            var database =
            mongoClient.GetDatabase(settings.Value.DatabaseName);

            _collection = database.GetCollection<Order>(_collectionName);
            _productServices = productServices;
        }

        /// <inheritdoc/>
        public async Task<Order> AddAsync(Order order)
        {
            await CheckProductQuantityInStockBeforeAdd(order);

            await ReduceProductQuantityInStockBasedOn(order);

            //CreateNewInvoice(order);

            await _collection.InsertOneAsync(order);

            return order;
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
        public async Task UpdateInvoiceStatusAsync(string orderId, Order order)
        {
            await _collection.ReplaceOneAsync(c => c.Id == orderId, order);
        }

        ///<inheritdoc/>
        public async Task<List<Order>> GetAllAsync()
        {
            var orders = await _collection.FindAsync(_ => true);
            return await orders.ToListAsync();
        }

        ///<inheritdoc/>
        public async Task<List<Order>> GetPendingOrderAsync()
        {
            var orders = await _collection.AsQueryable()
                .Where(o => o.Invoice.Status.Equals(InvoiceStatus.Pending)).ToListAsync();

            return orders;
        }
        ///<inheritdoc/>
        public async Task<Order> GetByIdAsync(string id)
        {
            return await _collection.FindAsync(o => o.Id == id).GetAwaiter().GetResult().FirstOrDefaultAsync();
        }

        ///<inheritdoc/>
        public async Task<List<Order>> GetByUserIdAsync(string userId)
        {
            var orders = await _collection.FindAsync(o => o.UserId == userId).GetAwaiter().GetResult().ToListAsync();

            return orders;
        }
    }
}
