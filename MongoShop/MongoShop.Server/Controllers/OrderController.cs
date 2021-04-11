using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoShop.BusinessDomain.Orders;
using MongoShop.BusinessDomain.Products;
using MongoShop.SharedModels.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MongoShop.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMapper mapper, 
            IOrderServices orderServices, 
            IProductServices productServices,
            ILogger<OrderController> logger)
        {
            _mapper = mapper;
            _orderServices = orderServices;
            this._productServices = productServices;
            _logger = logger;
        }

        /// <summary>
        /// Create new order.
        /// </summary>
        /// <param name="newOrder">New Order.</param>
        /// <returns>Status code 201 if created successfully.</returns>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> Create(CreateOrderRequest newOrder)
        {
            try
            {
                var order = _mapper.Map<Order>(newOrder);

                await AttachProductInfosToOrder(newOrder, order);

                order = await _orderServices.AddAsync(order);

                var createdOrder = _mapper.Map<OrderViewModel>(order);

                return CreatedAtAction(nameof(GetById), new { id = order.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when create product.");


                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error when attempt to create order");
            }
        }

        private async Task AttachProductInfosToOrder(CreateOrderRequest newOrder, Order order)
        {
            for (int i = 0; i < newOrder.OrderedProducts.Count; i++)
            {
                string productId = newOrder.OrderedProducts[i].ProductId;

                var product = await _productServices.GetByIdAsync(productId);

                order.OrderedProducts[i].Product = product;
            }
        }

        /// <summary>
        /// Get all Orders.
        /// </summary>
        /// <returns>List orders if any</returns>
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<List<OrderViewModel>>> GetAll()
        {

            try
            {
                var orders = await _orderServices.GetAllAsync();
                if (orders is null)
                {
                    _logger.LogInformation("There is no order available");

                    return NotFound("There is no order available");
                }

                var orderViewModels = _mapper.Map<List<OrderViewModel>>(orders);

                return Ok(orderViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getting all orders.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Changes invoice status to paid.
        /// </summary>
        /// <param name="id">Order id.</param>
        /// <returns>200 Ok.</returns>
        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                    nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> Pay([Required, StringLength(24, MinimumLength = 24, ErrorMessage = "Id must be 24 digits string")] string id)
        {
            try
            {
                var order = await _orderServices.GetByIdAsync(id);

                order.Invoice.Status = InvoiceStatus.Paid;

                await _orderServices.UpdateInvoiceStatusAsync(id, order);

                return Ok("Order is paid!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when pay order.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Cancel an order.
        /// </summary>
        /// <param name="id">Order id.</param>
        /// <returns>200 Ok.</returns>
        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                    nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> Cancel([Required, StringLength(24, MinimumLength = 24, ErrorMessage = "Id must be 24 digits string")] string id)
        {
            try
            {
                var order = await _orderServices.GetByIdAsync(id);

                order.Invoice.Status = InvoiceStatus.Cancel;

                await _orderServices.UpdateInvoiceStatusAsync(id, order);

                return Ok("Order is canceled!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when cancel order.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Gets order detail by id.
        /// </summary>
        /// <param name="id">Order id, must be 24 digits string.</param>
        /// <returns>A model of type <typeparamref name="OrderViewModel"/>></returns>
        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<OrderViewModel>> GetById([Required, StringLength(24, MinimumLength = 24, ErrorMessage = "Id must be 24 digits string")] string id)
        {

            try
            {
                var order = await _orderServices.GetByIdAsync(id);

                if (order is null)
                {
                    _logger.LogInformation("Order with Id = {id} not found", id);

                    return NotFound($"Order with Id = {id} not found");
                }

                var orderViewmodel = _mapper.Map<OrderViewModel>(order);

                return Ok(orderViewmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when get order id: {id}.", id);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Get all pending Orders.
        /// </summary>
        /// <returns>List orders if any</returns>
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<List<OrderViewModel>>> GetAllPending()
        {

            try
            {
                var orders = await _orderServices.GetPendingOrderAsync();
                if (orders is null || orders.Count == 0)
                {
                    _logger.LogInformation("There is no pending order available");

                    return NotFound("There is no pending order available");
                }

                var orderViewModels = _mapper.Map<List<OrderViewModel>>(orders);

                return Ok(orderViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getting all pending orders.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
    }
}
