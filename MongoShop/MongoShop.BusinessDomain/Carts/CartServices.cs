using System.Collections.Generic;
using System.Threading.Tasks;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;

namespace MongoShop.BusinessDomain.Carts
{
    public class CartServices : ICartServices
    {
        private readonly IUserServices _userServices;

        public CartServices(IUserServices userServices)
        {
            _userServices = userServices;
        }

        /// <inheritdoc/>
        public async Task<List<OrderedProduct>> GetCartItemsByUserIdAsync(string userId)
        {
            var user = await _userServices.GetActiveUserByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("user is not exists");
            }

            var cartItems = user.Cart.Products;

            return cartItems;
        }
        
        /// <inheritdoc/>
        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            var user = await _userServices.GetActiveUserByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("user is not exists");
            }

            var cart = user.Cart;

            return cart;
        }

        /// <inheritdoc/>     
        public async Task UpdateCartAsync(string userId,Cart cart)
        {
            var user = await _userServices.GetActiveUserByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("user is not exists");
            }

            user.Cart = cart;

            await _userServices.UpdateUserAsync(userId, user);
        }

        public async Task ClearCart(string userId)
        {
            var user = await _userServices.GetActiveUserByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("user is not exists");
            }

            user.Cart = new Cart();

            await _userServices.UpdateUserAsync(userId, user);
        }
    }
}
