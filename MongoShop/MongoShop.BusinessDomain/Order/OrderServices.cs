using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Order
{
    public class OrderServices : IOrderServices
    {
        private readonly IMongoCollection<Order> _collection;
        private readonly IDatabaseSetting _databaseSetting;
        private const string CollectionName = "order";

        public OrderServices(IDatabaseSetting databaseSetting)
        {
            _databaseSetting = databaseSetting;

            var client = new MongoClient(_databaseSetting.ConnectionString);
            var database = client.GetDatabase(_databaseSetting.DatabaseName);

            _collection = database.GetCollection<Order>(CollectionName);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Order order)
        {
            order.Status = false;
            await _collection.InsertOneAsync(order);
        }

        /// <inheritdoc/>
        public Task DeleteAsync(string id, Order order)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task EditAsync(string id, Order order)
        {
            order.Status = true;
            await _collection.ReplaceOneAsync(c => c.Id == id, order);
        }

        ///<inheritdoc/>
        public async Task<List<Order>> GetAllAsync()
        {
            var _list = await _collection.FindAsync(_ => true);
            return await _list.ToListAsync();
        }
    }
}
