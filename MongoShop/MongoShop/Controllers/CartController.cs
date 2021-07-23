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
    
                //Không hiểu sao cái Total đã tính r lại ko lưu dc, khó hiểu vl
                order = _mapper.Map<BusinessDomain.Orders.Order>(cartCheckoutViewModel);
                order.PhoneNumber = cartCheckoutViewModel.PhoneNumber;
                order.ShipAddress.City = cartCheckoutViewModel.City;
                order.ShipAddress.Number = cartCheckoutViewModel.AddressNumber;
                order.ShipAddress.Street = cartCheckoutViewModel.Street;

                string userId = GetCurrentLoggedInUserId();
                order.UserId = userId;

                var cartItems = await _cartServices.GetItemsByUserIdAsync(userId);
                order.OrderedProducts = cartItems;

                //Tính tổng
                var total = CalculateTotalPrice(cartItems);
                order.Total = (double)total;

                order.CreatedTime = DateTime.Now;

                order.Invoice = new Invoice()
                {
                    PaymentMethod = BusinessDomain.Orders.PaymentMethod.ShipCod,
                    Status = BusinessDomain.Orders.InvoiceStatus.Pending
                };
                order.ShipppingFee = 45000.00;
                
                // save order to database
                await _orderServices.AddAsync(order);
                await _cartServices.ClearCartAsync(userId);
                return Redirect("/Cart/CheckoutSuccess/" + order.Id);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return await Index();
            }
        }

        [Route("/Cart/CheckoutSuccess/{orderId}")]
        public async Task<IActionResult> CheckoutSuccess(string orderID)
        {
            var order = await _orderServices.GetByIdAsync(orderID);

            return View(order);
        }

        public async Task<SmartButtonHttpResponse> PaypalCheckout()
        {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            string userId = GetCurrentLoggedInUserId();
            Cart cartFromDb = await _cartServices.GetByUserIdAsync(userId);

            var paypal_orderId = DateTime.Now.Ticks;

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
                        Value = usd_value.ToString()
                    },
                    Quantity = item.OrderedQuantity.ToString(),
                    Sku = item.Product.Id,
                    Tax = new Money
                    {
                        CurrencyCode = "USD",
                        Value = "0"
                    },
                });
            }

            var shipping_fee = Math.Round(45000.00 / 22000, 2);

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
                            Value = (Math.Round(total,2) + shipping_fee).ToString(),
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = Math.Round(total,2).ToString()
                                },
                                Shipping = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = shipping_fee.ToString()
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

                order.Total = Math.Round(Convert.ToDouble(result.PurchaseUnits[0].AmountWithBreakdown.AmountBreakdown.ItemTotal.Value) * 22000, 0);
                order.ShipppingFee = Math.Round(Convert.ToDouble(result.PurchaseUnits[0].AmountWithBreakdown.AmountBreakdown.Shipping.Value) * 22000, 0);

                // save order to database
                await _orderServices.AddAsync(order);
                await _cartServices.ClearCartAsync(userId);
                return View(order);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                //Execute Refund Procedure
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

        public IActionResult CheckoutFailed()
        {
            return View();
        }
    }
}
