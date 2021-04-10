using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Orders
{
    public interface IOrderServices
    {
        /// <summary>
        /// Get all Orders
        /// </summary>
        /// <returns></returns>
        Task<List<Order>> GetAllAsync();

        /// <summary>
        /// Add a new Order accompany with invoice and reduce the quantity in stock.
        /// Throw exception if any product not have enough quantity in stock.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<Order> AddAsync(Order order);

        /// <summary>
        /// Update an Order. Use for update invoice status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Task UpdateInvoiceStatusAsync(string id, Order order);

        /// <summary>
        /// Get orders with invoice that has status 'Pending'
        /// </summary>
        /// <returns></returns>
        Task<List<Order>> GetPendingOrderAsync();

        /// <summary>
        /// Get order by order id.
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns></returns>
        Task<Order> GetByIdAsync(string id);
    }
}
