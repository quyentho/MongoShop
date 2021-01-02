using System.Collections.Generic;
using System.Threading.Tasks;
using MongoShop.BusinessDomain.Carts;
using MongoShop.BusinessDomain.Products;

namespace MongoShop.BusinessDomain.Carts
{
    public interface ICartServices
    {
        /// <summary>
        /// Get all cart items of the user provided by userId and  user.Status = true.
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetCartItemsByUserIdAsync(string userId);

        /// <summary>
        /// Add a new cart
        /// </summary>
        /// <param name="cart">cart to add.</param>
        /// <returns></returns>
        Task UpdateCartAsync(string userId, Cart cart);
    }
}
