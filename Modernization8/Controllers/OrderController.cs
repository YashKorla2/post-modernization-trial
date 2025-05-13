using Modernization8.Constants;
using Modernization8.Models;
using Microsoft.AspNetCore.Mvc;

namespace Modernization8.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        List<List<Order>> orders = OrderConstants.Orders;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index(string searchTerm)
        {
            var filteredOrders = string.IsNullOrEmpty(searchTerm) 
            ? orders
            : orders.Select(orderList => orderList
                    .Where(cart => cart.ProductName.ToLower().Contains(searchTerm.ToLower()))
                    .ToList()
                )
                .Where(filteredList => filteredList.Count > 0)
                .ToList();

            ViewData["OrderCount"] = orders.Count;
            ViewData["SearchTerm"] = searchTerm;

            return View(filteredOrders);
        }

    }
}