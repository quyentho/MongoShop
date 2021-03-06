﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MongoShop.BusinessDomain.Wishlists;
using MongoShop.BusinessDomain.Products;

namespace MongoShop.BusinessDomain.Wishlists
{
    public interface IWishlistServices
    {
        /// <summary>
        /// Get all wishlist items of the user provided by userId and  user.Status = true.
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetWishlistItemsByUserIdAsync(string userId);

        /// <summary>
        /// Add or update wishlist
        /// </summary>
        /// <param name="wishlist">wishlist to add.</param>
        /// <returns></returns>
        Task AddOrUpdateAsync(string userId, Wishlist wishlist);

        /// <summary>
        /// Get wishlist by user id.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        Task<Wishlist> GetByUserIdAsync(string userId);

    }
}
