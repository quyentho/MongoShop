using System.Collections.Generic;
using System.Threading.Tasks;
using MongoShop.BusinessDomain.Carts;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;

namespace MongoShop.BusinessDomain.Carts
{
    public interface ICartServices
    {
        /// <summary>
        /// Get cart by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Cart> GetByUserIdAsync(string userId);

        /// <summary>
        /// Get all cart items of the user provided by userId and  user.Status = true.
        /// </summary>
        /// <returns></returns>
        Task<List<OrderedProduct>> GetItemsByUserIdAsync(string userId);

        /// <summary>
        /// Add a new cart
        /// </summary>
        /// <param name="cart">cart to add.</param>
        /// <returns></returns>
        Task AddOrUpdateAsync(string userId, Cart cart);

        /// <summary>
        /// Clear all items in cart.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task ClearCartAsync(string userId);
    }
}
