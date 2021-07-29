using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MongoShop.BusinessDomain.Carts;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.Infrastructure.Extensions;

namespace MongoShop.Utils
{
    public static class CartHelper
    {
         public static async Task<Cart> GetCartFromSession(HttpContext httpContext, IProductServices productService)
        {
            Cart cart = new Cart();

            List<string> listShoppingCart = httpContext.Session.Get<List<string>>("ssShoppingCart");

            
            if (listShoppingCart != null)
            {
                foreach (var productId in listShoppingCart)
                {
                    var productFromDb = await productService.GetByIdAsync(productId);

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

          public static async Task SetCartCount(
              HttpContext httpContext, 
          IProductServices productServices,
          UserManager<ApplicationUser> userManager,
          ICartServices cartServices,
          ViewDataDictionary ViewData)
        {
            string userId = userManager.GetUserId(httpContext.User);
            if (!string.IsNullOrEmpty(userId))
            {
                ViewData["CartCount"] = 0;

                var cartFromSession = await GetCartFromSession(httpContext, productServices);

                int cartCount = 0;
                if (cartFromSession != null && cartFromSession.Products != null && cartFromSession.Products.Count != 0)
                {
                    foreach (var product in cartFromSession.Products)
                    {
                        cartCount += product.OrderedQuantity ;
                    }

                    ViewData["CartCount"] = cartCount;
                }

                List<OrderedProduct> cartItems = await cartServices.GetItemsByUserIdAsync(userId);
                
                if (cartItems != null && cartItems.Count != 0)
                {
                    
                    foreach (var item in cartItems)
                    {
                        cartCount += item.OrderedQuantity;
                    }

                    ViewData["CartCount"] = cartCount;
                }
            }
        }
    }
}