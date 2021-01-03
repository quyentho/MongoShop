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
using MongoShop.BusinessDomain.Carts;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.Extensions;
using MongoShop.Models.Cart;

namespace MongoShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICartServices _cartServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public CartController(IProductServices productServices, ICartServices cartServices, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _productServices = productServices;
            _cartServices = cartServices;
            _userManager = userManager;
            this._mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            string userId = GetCurrentLoggedInUserId();

            Cart cartFromDb = await _cartServices.GetCartByUserIdAsync(userId);

            if (cartFromDb is null)
            {
                cartFromDb = new Cart();
            }

            Cart cartFromSession = await GetCartFromSession();

            if (cartFromSession != null && cartFromSession.Products.Count > 0)
            {
                cartFromDb.Products.AddRange(cartFromSession.Products);
            }

            CalculateTotalPrice(cartFromDb);

            await _cartServices.UpdateCartAsync(userId, cartFromDb);

            // clear session
            HttpContext.Session.Clear();

            var cartIndexViewModel = _mapper.Map<CartIndexViewModel>(cartFromDb);

            return View(cartIndexViewModel);
        }

        private static void CalculateTotalPrice(Cart cartFromDb)
        {
            // sum price * quantity for each product. If product is null then total = 0.
            cartFromDb.Total = cartFromDb.Products?.Sum(o => o.Product.Price * o.OrderedQuantity) ?? 0;
        }

        private async Task<Cart> GetCartFromSession()
        {
            Cart cart = new Cart();

            List<string> listShoppingCart = HttpContext.Session.Get<List<string>>("ssShoppingCart");

            if (listShoppingCart != null)
            {
                foreach (var productId in listShoppingCart)
                {
                    var productFromDb = await _productServices.GetByIdAsync(productId);
                    cart.Products.Add(new OrderedProduct()
                    {
                        OrderedQuantity = 1,
                        Product = productFromDb
                    });
                    cart.Total += productFromDb.Price;
                }
            }

            return cart;
        }

        private string GetCurrentLoggedInUserId()
        {
            string userId = _userManager.GetUserId(HttpContext.User);

            return userId;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddToCart(string productId)
        {
            List<string> lstShoppingCart = HttpContext.Session.Get<List<string>>("ssShoppingCart");
            if (lstShoppingCart == null)
            {
                lstShoppingCart = new List<string>();
            }
            lstShoppingCart.Add(productId);
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            return RedirectToAction("Index", "Customer");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCartAsync(string productId)
        {
            List<string> listShoppingCart = HttpContext.Session.Get<List<string>>("ssShoppingCart");

            if (listShoppingCart != null)
            {
                if (listShoppingCart.Contains(productId))
                {
                    listShoppingCart.Remove(productId);

                    // set session back after remove
                    HttpContext.Session.Set("ssShoppingCart", listShoppingCart);

                    string userId = GetCurrentLoggedInUserId();

                    // get cart from db to collect cart id.
                    Cart cartFromDb = await _cartServices.GetCartByUserIdAsync(userId);

                    Cart cartFromSession = await GetCartFromSession();

                    cartFromSession.Id = cartFromDb.Id;

                    await _cartServices.UpdateCartAsync(userId, cartFromSession);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
