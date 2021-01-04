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
        private readonly IOrderServices _orderServices;

        public CartController(IProductServices productServices, 
            ICartServices cartServices, 
            UserManager<ApplicationUser> userManager, 
            IMapper mapper,
            IOrderServices orderServices)
        {
            _productServices = productServices;
            _cartServices = cartServices;
            _userManager = userManager;
            this._mapper = mapper;
            this._orderServices = orderServices;
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

        //public async Task<IActionResult> UpdateProductQuantityAsync(string productId, int quantity)
        //{
        //    string userId = GetCurrentLoggedInUserId();

        //    Cart cartFromDb = await _cartServices.GetCartByUserIdAsync(userId);

        //    var listProductInCart = await _cartServices.GetCartItemsByUserIdAsync(userId);

        //    var product = listProductInCart.Where(p => p.Product.Id == productId).FirstOrDefault();
        //    product.OrderedQuantity = quantity;

        //    cartFromDb.Products.Update

        //}

        [HttpPost]
        public async Task<IActionResult> RemoveFromCartAsync(string productId)
        {

            string userId = GetCurrentLoggedInUserId();

            // get cart from db to collect cart id.
            Cart cartFromDb = await _cartServices.GetCartByUserIdAsync(userId);

            var productToRemove = cartFromDb.Products.Find(p => p.Product.Id == productId);

            cartFromDb.Products.Remove(productToRemove);

            await _cartServices.UpdateCartAsync(userId, cartFromDb);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutAsync([FromForm] CartIndexViewModel viewModel)
        {
            var cartCheckoutViewModel = _mapper.Map<CartCheckoutViewModel>(viewModel);

            string userId = GetCurrentLoggedInUserId();
            Cart cart = await _cartServices.GetCartByUserIdAsync(userId);

            cart.Products = viewModel.Products;

            // update product order quantity.
            await _cartServices.UpdateCartAsync(userId, cart);

            return View(cartCheckoutViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromForm] CartCheckoutViewModel cartCheckoutViewModel)
        {
            try
            {
                var order = new Order();

                order = _mapper.Map<Order>(cartCheckoutViewModel);

                string userId = GetCurrentLoggedInUserId();
                order.UserId = userId;

                var cartItems = await _cartServices.GetCartItemsByUserIdAsync(userId);
                order.OrderedProducts = cartItems;

                // save order to database
                await _orderServices.AddAsync(order);
                await _cartServices.ClearCart(userId);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return await Index();
            }
        }
    }
}
