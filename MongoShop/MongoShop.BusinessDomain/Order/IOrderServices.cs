using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Order
{
    public interface IOrderServices
    {
        /// <summary>
        /// Get all Orders
        /// </summary>
        /// <returns></returns>
        Task<List<Order>> GetAllAsync();

        /// <summary>
        /// Add a new Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task AddAsync(Order order);

        /// <summary>
        /// Update an Order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Task EditAsync(string id, Order order);

        /// <summary>
        /// Delete Order??? Maybe No use
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Task DeleteAsync(string id, Order order);
    }
}
