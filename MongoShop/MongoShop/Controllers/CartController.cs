using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
//using BraintreeHttp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoShop.BusinessDomain.Carts;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.PayPal;
using MongoShop.BusinessDomain.Products;
using MongoShop.BusinessDomain.Users;
using MongoShop.Infrastructure.Extensions;
using MongoShop.Models.Cart;
//using PayPal.Core;
//using PayPal.v1.Payments;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;


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
        private readonly string _clientId;
        private readonly string _secretKey;

        //public double tyGiaUSD = 23300;

        public CartController(IProductServices productServices, 
            ICartServices cartServices, 
            UserManager<ApplicationUser> userManager, 
            IMapper mapper,
            IOrderServices orderServices,
            IConfiguration config)
        {
            _productServices = productServices;
            _cartServices = cartServices;
            _userManager = userManager;
            this._mapper = mapper;
            this._orderServices = orderServices;
            _clientId = config["PaypalSettings:ClientID"];
            _secretKey = config["PaypalSettings:SecretKey"];
        }

        public async Task<IActionResult> Index()
        {
            string userId = GetCurrentLoggedInUserId();

            Cart cartFromDb = await _cartServices.GetByUserIdAsync(userId);

            if (cartFromDb is null)
            {
                cartFromDb = new Cart();
            }

            if (cartFromDb.Products is null)
            {
                cartFromDb.Products = new List<OrderedProduct>();
            }

            Cart cartFromSession = await GetCartFromSession();

            if (cartFromSession != null && cartFromSession.Products.Count > 0)
            {
                foreach (var _product in cartFromSession.Products)
                {
                    //Avoid Duplicate and Add-on
                    if (cartFromDb.Products.Any(m => m.Product.Id == _product.Product.Id))
                    {
                        var _targetProduct = cartFromDb.Products.FirstOrDefault(m => m.Product.Id == _product.Product.Id);
                        _targetProduct.OrderedQuantity += 1;
                    }
                    else
                    {
                        cartFromDb.Products.Add(new OrderedProduct
                        {
                            Product = _product.Product,
                            OrderedQuantity = _product.OrderedQuantity
                        });
                    }
                }               
            }

            cartFromDb.Total = CalculateTotalPrice(cartFromDb.Products);

            await _cartServices.AddOrUpdateAsync(userId, cartFromDb);

            // clear session
            HttpContext.Session.Clear();

            var cartIndexViewModel = _mapper.Map<CartIndexViewModel>(cartFromDb);

            return View("Index",cartIndexViewModel);
        }

        private static double? CalculateTotalPrice(List<OrderedProduct> products)
        {
            // sum price * quantity for each product. If product is null then total = 0.
            return products?.Sum(o => o.Product.Price * o.OrderedQuantity) ?? 0;
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
        public IActionResult Add(string productId)
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
        public async Task<IActionResult> Remove(string productId)
        {

            string userId = GetCurrentLoggedInUserId();

            // get cart from db to collect cart id.
            Cart cartFromDb = await _cartServices.GetByUserIdAsync(userId);

            var productToRemove = cartFromDb.Products.Find(p => p.Product.Id == productId);

            cartFromDb.Products.Remove(productToRemove);

            await _cartServices.AddOrUpdateAsync(userId, cartFromDb);

            return RedirectToAction(nameof(Index));
        }

        //Bỏ Post đê
        [HttpPost]
        public async Task<IActionResult> Checkout([FromForm] CartIndexViewModel viewModel)
        {
            var cartCheckoutViewModel = _mapper.Map<CartCheckoutViewModel>(viewModel);
            cartCheckoutViewModel.Total = CalculateTotalPrice(cartCheckoutViewModel.Products);
            string userId = GetCurrentLoggedInUserId();
            Cart cart = await _cartServices.GetByUserIdAsync(userId);

            
            cart.Products = viewModel.Products;
            cart.Total = cartCheckoutViewModel.Total;
            // update product order quantity.
            await _cartServices.AddOrUpdateAsync(userId, cart);

            return View(cartCheckoutViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromForm] CartCheckoutViewModel cartCheckoutViewModel)
        {
            try
            {
                var order = new BusinessDomain.Orders.Order();

                    order = _mapper.Map<BusinessDomain.Orders.Order>(cartCheckoutViewModel);

                string userId = GetCurrentLoggedInUserId();
                order.UserId = userId;

                var cartItems = await _cartServices.GetItemsByUserIdAsync(userId);
                order.OrderedProducts = cartItems;

                // save order to database
                await _orderServices.AddAsync(order);
                await _cartServices.ClearCartAsync(userId);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return await Index();
            }
        }

        public async Task<SmartButtonHttpResponse> PaypalCheckout()
        {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            string userId = GetCurrentLoggedInUserId();
            Cart cartFromDb = await _cartServices.GetByUserIdAsync(userId);

            var total = cartFromDb.Total;
            var paypal_orderId = DateTime.Now.Ticks;

            //var itemList = new ItemList()
            //{
            //    Items = new List<Item>()
            //};

            var ItemList = new List<Item>();
            foreach (var item in cartFromDb.Products)
            {
                ItemList.Add(new Item()
                {
                    Name = item.Product.Name,
                    UnitAmount = new Money
                    {
                        CurrencyCode = "USD",
                        Value = item.Product.Price.ToString()
                    },
                    Quantity = item.OrderedQuantity.ToString(),
                    Sku = "sku",
                    Tax = new Money
                    {
                        CurrencyCode = "USD",
                        Value = "0"
                    },
                   //Category = item.Product.Category.Name.ToString()
                });
            }

            OrderRequest orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",

                ApplicationContext = new ApplicationContext
                {
                    BrandName = "MongoShop",
                    CancelUrl = "https://localhost:44361/Cart/CheckoutFailed",
                    ReturnUrl = "https://localhost:44361/Cart/CheckoutSuccess",
                    UserAction = "CONTINUE"
                },

                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        ReferenceId = paypal_orderId.ToString(),
                        Description = $"Invoice #{paypal_orderId}",
                        SoftDescriptor = "Clothings",
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            Value = total.ToString(),
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = total.ToString()
                                }
                            }

                        },
                        Items = ItemList

                    }
                }
            };

            var request = new OrdersCreateRequest();
            request.Headers.Add("prefer", "return=representation");
            request.RequestBody(orderRequest);

            //try
                var response = await client.Execute(request);
                //var statusCode = response.StatusCode;
                PayPalCheckoutSdk.Orders.Order result = response.Result<PayPalCheckoutSdk.Orders.Order>();

                var payPalHttpResponse = new SmartButtonHttpResponse(response)
                {
                    orderID = result.Id
                };

                return payPalHttpResponse;

                ////var links = result.Links.GetEnumerator();
                //string paypalRedirectUrl = null;

                //foreach(LinkDescription link in result.Links)
                //{
                //    if(link.Rel.ToLower().Equals("approve"))
                //    {
                //        paypalRedirectUrl = link.Href;

                //    }
                //}

                //////while (links.MoveNext())
                //////{
                //////    LinkDescription lnk = links.Current;
                //////    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                //////    {
                ////        paypalRedirectUrl = lnk.Href;
                //////    }
                //////}

                //return Redirect(paypalRedirectUrl);
            //catch

                //var statusCode = httpException.StatusCode;
                //var debugId = httpException.Headers.GetValues("Paypal-Debug-Id").FirstOrDefault();

                //return Redirect("/Cart/CheckoutFailed");
            



            ////var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";



            //var payment = new Payment()
            //{
            //    Intent = "sale",
            //    Transactions = new List<Transaction>()
            //    {
            //        new Transaction()
            //        {
            //            Amount = new Amount()
            //            {
            //                Total = total.ToString(),
            //                Currency = "USD",
            //                Details = new AmountDetails()
            //                {
            //                    Tax = "0",
            //                    Shipping = "0",
            //                    Subtotal = total.ToString()
            //                }
            //            },
            //            ItemList = itemList,
            //            Description = $"Invoice #{paypal_orderId}",
            //            InvoiceNumber = paypal_orderId.ToString()
            //        }
            //    },
            //    RedirectUrls = new RedirectUrls()
            //    {
            //        CancelUrl = "https://localhost:44361/Cart/CheckoutFailed",
            //        ReturnUrl = "https://localhost:44361/Cart/CheckoutSuccess"
            //    },
            //    Payer = new Payer()
            //    {
            //        PaymentMethod = "paypal"
            //    }
            //};


            //PaymentCreateRequest request = new PaymentCreateRequest();
            //request.RequestBody(payment);


        }

        //public IActionResult Captured(string orderID)
        //{
        //    return Ok();
        //}
        

        public IActionResult CheckoutFailed()
        {
            return View();
        }

        public IActionResult CheckoutSuccess(string orderID)
        {
            return View();
        }

    }
}
