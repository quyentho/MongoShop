using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoShop.Areas.Admin.ViewModels.Order;
using MongoShop.BusinessDomain.Orders;

namespace MongoShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;
        public OrderController(IOrderServices orderServices, IMapper mapper)
        {
            _orderServices = orderServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderServices.GetOrdersWithUnpaidInvoiceAsync();

            var indexOrderViewModels = _mapper.Map<List<IndexOrderViewModel>>(orders);

            return View(indexOrderViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Paid(string id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);

            order.Invoice.Status = InvoiceStatus.Paid;

            await _orderServices.UpdateInvoiceStatusAsync(id, order);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(string id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);

            order.Invoice.Status = InvoiceStatus.Cancel;

            await _orderServices.UpdateInvoiceStatusAsync(id, order);

            return RedirectToAction(nameof(Index));
        }
    }
}
