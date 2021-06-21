using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoShop.Areas.Admin.ViewModels.Order;
using MongoShop.BusinessDomain.Orders;
using MongoShop.Utils;

namespace MongoShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
        public async Task<IActionResult> Index(int currentPageNumber = 1)
        {
            var orders = await _orderServices.GetPendingOrderAsync();

            var indexOrderViewModels = _mapper.Map<List<IndexOrderViewModel>>(orders);

            return View(PaginatedList<IndexOrderViewModel>.CreateAsync(indexOrderViewModels.AsQueryable(), currentPageNumber));
        }

        [HttpGet]
        public async Task<IActionResult> Paid(string id)
        {
            var order = await _orderServices.GetByIdAsync(id);

            order.Invoice.Status = InvoiceStatus.Paid;

            await _orderServices.UpdateInvoiceStatusAsync(id, order);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(string id)
        {
            var order = await _orderServices.GetByIdAsync(id);

            order.Invoice.Status = InvoiceStatus.Cancel;

            await _orderServices.UpdateInvoiceStatusAsync(id, order);

            return RedirectToAction(nameof(Index));
        }
    }
}
