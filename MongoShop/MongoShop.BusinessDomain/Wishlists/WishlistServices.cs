using System.Collections.Generic;
using System.Threading.Tasks;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;

namespace MongoShop.BusinessDomain.Wishlists
{
    public class WishlistServices : IWishlistServices
    {
        private readonly IUserServices _userServices;

        public WishlistServices(IUserServices userServices)
        {
            _userServices = userServices;
        }

        /// <inheritdoc/>
        public async Task<List<Product>> GetWishlistItemsByUserIdAsync(string userId)
        {
            var user = await _userServices.GetActiveUserByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("user is not exists");
            }

            var wishlistItems = user.Wishlist.Products;

            return wishlistItems;
        }

        /// <inheritdoc/>     
        public async Task AddOrUpdateAsync(string userId,Wishlist wishlist)
        {
            var user = await _userServices.GetActiveUserByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("user is not exists");
            }

            user.Wishlist = wishlist;

            await _userServices.UpdateUserAsync(userId, user);
        }

        /// <inheritdoc/>
        public async Task<Wishlist> GetByUserIdAsync(string userId)
        {
            var user = await _userServices.GetActiveUserByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("user is not exists");
            }

            var wishlist = user.Wishlist;

            return wishlist;
        }
    }
}
