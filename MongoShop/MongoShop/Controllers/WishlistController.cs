using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoShop.BusinessDomain.Wishlists;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.Models.Wishlist;

namespace MongoShop.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IWishlistServices _wishlistServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IOrderServices _orderServices;

        public WishlistController(IProductServices productServices,
            IWishlistServices wishlistServices,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IOrderServices orderServices)
        {
            _productServices = productServices;
            _wishlistServices = wishlistServices;
            _userManager = userManager;
            this._mapper = mapper;
            this._orderServices = orderServices;
        }

        public async Task<IActionResult> Index()
        {
            string userId = GetCurrentLoggedInUserId();

            Wishlist wishlistFromDb = await _wishlistServices.GetByUserIdAsync(userId);

            if (wishlistFromDb is null)
            {
                wishlistFromDb = new Wishlist();
            }

            if (wishlistFromDb.Products is null)
            {
                wishlistFromDb.Products = new List<Product>();
            }

            var wishlistIndexViewModel = _mapper.Map<WishlistIndexViewModel>(wishlistFromDb);

            return View(wishlistIndexViewModel);
        }

        private string GetCurrentLoggedInUserId()
        {
            string userId = _userManager.GetUserId(HttpContext.User);

            return userId;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add(string productId)
        {
            string userId = GetCurrentLoggedInUserId();
            Wishlist userWishlist = await _wishlistServices.GetByUserIdAsync(userId);
            if (userWishlist is null)
            {
                userWishlist = new Wishlist();
            }

            var newProductWishlist = await _productServices.GetByIdAsync(productId);

            if (!userWishlist.Products.Any(p=>p.Id == newProductWishlist.Id))
            {
                userWishlist.Products.Add(newProductWishlist);

                await _wishlistServices.AddOrUpdateAsync(userId, userWishlist);
            }

            return RedirectToAction("Index", "Customer");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string productId)
        {

            string userId = GetCurrentLoggedInUserId();

            Wishlist wishlistFromDb = await _wishlistServices.GetByUserIdAsync(userId);

            var productToRemove = wishlistFromDb.Products.Find(p => p.Id == productId);

            wishlistFromDb.Products.Remove(productToRemove);

            await _wishlistServices.AddOrUpdateAsync(userId, wishlistFromDb);

            return RedirectToAction(nameof(Index));
        }
    }
}
