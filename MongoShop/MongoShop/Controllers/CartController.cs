using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentEmail.Core;
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
//using PayPalCheckoutSdk.Payments;
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

        private readonly IFluentEmail _emailSender;

        //public double tyGiaUSD = 23300;

        public CartController(IProductServices productServices, 
            ICartServices cartServices, 
            UserManager<ApplicationUser> userManager, 
            IMapper mapper,
            IOrderServices orderServices,
            IFluentEmail emailSender,
            IConfiguration config)
        {
            _productServices = productServices;
            _cartServices = cartServices;
            _userManager = userManager;
            this._mapper = mapper;
            _emailSender = emailSender;
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

        [HttpPost]
        public async Task<IActionResult> Checkout([FromForm] CartIndexViewModel viewModel)
        {
            var cartCheckoutViewModel = _mapper.Map<CartCheckoutViewModel>(viewModel);
            cartCheckoutViewModel.Total = CalculateTotalPrice(cartCheckoutViewModel.Products);
            string userId = GetCurrentLoggedInUserId();
            Cart cart = await _cartServices.GetByUserIdAsync(userId);

            
            cart.Products = cartCheckoutViewModel.Products;
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
                order.CreatedTime = DateTime.Now;

                var cartItems = await _cartServices.GetItemsByUserIdAsync(userId);
                order.OrderedProducts = cartItems;

                order.CreatedTime = DateTime.Now;
                order.Invoice = new Invoice()
                {
                    PaymentMethod = BusinessDomain.Orders.PaymentMethod.ShipCod,
                    Status = BusinessDomain.Orders.InvoiceStatus.Pending
                };

                // save order to database
                await _orderServices.AddAsync(order);
                await _cartServices.ClearCartAsync(userId);

                await SendOrderEmail(order);

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return await Index();
            }
        }

        private async Task SendOrderEmail(BusinessDomain.Orders.Order order)
        {
            var email = HttpContext.User.Identity.Name;

            // clear datetime cached
            System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();
            var body = "<div>"
                    + $"<h1>Bạn đã đặt hàng thành công vào lúc {DateTime.Now}</h1>"
                    + "</div>"
                    + "<div>"
                    + "<h2>Đơn hàng gồm</h2>"
                    + "<ul>";
            foreach (var item in order.OrderedProducts)
            {
                body += $"<li>{item.Product.Name}&nbsp;:{item.Product.Price}x{item.OrderedQuantity}&nbsp;={item.Product.Price * item.OrderedQuantity}</li>";
            }
            body += "</ul>";
            body += $"<h2>Tổng giá trị đơn hàng: <b>{order.Total}</b></h2></div>";
            await _emailSender
                    .To(email).Subject("Xác nhận đặt hàng tại MongoShop")
                    .Body(body, true)
                    .SendAsync();
        }

        public async Task<SmartButtonHttpResponse> PaypalCheckout()
        {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            string userId = GetCurrentLoggedInUserId();
            Cart cartFromDb = await _cartServices.GetByUserIdAsync(userId);

            //var total = Math.Round((double)(cartFromDb.Total / 22000), 2);
            //var total = cartFromDb.Total.ToString();
            var paypal_orderId = DateTime.Now.Ticks;

            //var itemList = new ItemList()
            //{
            //    Items = new List<Item>()
            //};
            double total = 0;
            var ItemList = new List<Item>();
            foreach (var item in cartFromDb.Products)
            {
                var usd_value = Math.Round(item.Product.Price / 22000, 2);
                var item_quantity = item.OrderedQuantity;
                total += usd_value* item_quantity;
                ItemList.Add(new Item()
                {
                    Name = item.Product.Name,
                    UnitAmount = new Money
                    {
                        CurrencyCode = "USD",
                        //Value = item.Product.Price.ToString()
                        Value = usd_value.ToString()
                    },
                    Quantity = item.OrderedQuantity.ToString(),
                    Sku = item.Product.Id,
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
                        //InvoiceId = paypal_orderId.ToString(),
                        //CustomId = paypal_orderId.ToString(),
                        SoftDescriptor = "Clothings",
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            Value = Math.Round(total,2).ToString(),
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = Math.Round(total,2).ToString()
                                }
                            }

                        },
                        Items = ItemList

                    }

                },
            };

            var request = new OrdersCreateRequest();
            request.Headers.Add("prefer", "return=representation");
            request.RequestBody(orderRequest);

            var response = await client.Execute(request);

                PayPalCheckoutSdk.Orders.Order result = response.Result<PayPalCheckoutSdk.Orders.Order>();

                var payPalHttpResponse = new SmartButtonHttpResponse(response)
                {
                    orderID = result.Id
                };

                return payPalHttpResponse;



        }

        public IActionResult CheckoutFailed()
        {
            return View();
        }

        [Route("/Cart/CheckoutSuccess/{orderId}/{captureId}")]
        public async Task<IActionResult> CheckoutSuccess(string orderId ,string captureId, [FromForm] CartCheckoutViewModel cartCheckoutViewModel)
        {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            OrdersGetRequest request = new OrdersGetRequest(orderId);
            var response = await client.Execute(request);
            var result = response.Result<PayPalCheckoutSdk.Orders.Order>();
            try
            {
                //Create a new order&invoice in mongodb
                var order = new BusinessDomain.Orders.Order();
                //Mapper doesn't work because haven't call any asp-action via Paypal Checkout Button, cartCheckoutViewModel will be null
                order = _mapper.Map<BusinessDomain.Orders.Order>(cartCheckoutViewModel);

                //order.PhoneNumber = result.Payer;
                order.ShipAddress.Street = result.PurchaseUnits[0].ShippingDetail.AddressPortable.AddressLine1;
                order.ShipAddress.City = result.PurchaseUnits[0].ShippingDetail.AddressPortable.AdminArea2;

                string userId = GetCurrentLoggedInUserId();
                order.UserId = userId;


                var cartItems = await _cartServices.GetItemsByUserIdAsync(userId);

                order.OrderedProducts = cartItems;

                order.Invoice = new Invoice
                {
                    PaymentMethod = BusinessDomain.Orders.PaymentMethod.PayPal,
                    Status = BusinessDomain.Orders.InvoiceStatus.Paid
                };

                order.CreatedTime = Convert.ToDateTime(result.CreateTime);

                order.Total = Math.Round(Convert.ToDouble(result.PurchaseUnits[0].AmountWithBreakdown.Value) * 22000, 0);
                // save order to database
                await _orderServices.AddAsync(order);
                await _cartServices.ClearCartAsync(userId);

                await SendOrderEmail(order);

                return View(order);
            }
            catch(ArgumentOutOfRangeException)
            {
                //Execute Refund Procedure


                //"94500092JB986153M"

                PayPalCheckoutSdk.Payments.CapturesRefundRequest newrequest = new PayPalCheckoutSdk.Payments.CapturesRefundRequest(captureId);
                PayPalCheckoutSdk.Payments.RefundRequest refundRequest = new PayPalCheckoutSdk.Payments.RefundRequest()
                {
                    Amount = new PayPalCheckoutSdk.Payments.Money
                    {
                        Value = result.PurchaseUnits[0].AmountWithBreakdown.Value,
                        CurrencyCode = result.PurchaseUnits[0].AmountWithBreakdown.CurrencyCode
                    }
                };

                newrequest.RequestBody(refundRequest);

                await client.Execute(newrequest);
                    
                return Redirect("/Cart/CheckoutFailed");
            }


        }

        public ActionResult CartSummary(int currentCount = 0)
        {   
            ViewData["CartCount"] = ++currentCount;

            return PartialView("Views/Shared/CartSummary.cshtml");
        }

    }
}
