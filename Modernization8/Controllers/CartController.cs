using Modernization8.Models;
using Modernization8.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Modernization8.Controllers;

public class CartController : Controller
{
    private readonly ILogger<CartController> _logger;
    List<Cart> carts = CartConstants.Carts;
    List<List<Order>> orders = OrderConstants.Orders;

    public CartController(ILogger<CartController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(string searchTerm)
    {
        var filteredCarts = string.IsNullOrEmpty(searchTerm) 
            ? carts
            : carts.Where(
                p => p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        return View(filteredCarts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var cart = carts.FirstOrDefault(x => x.ProductId == id);
        if (cart != null)
        {
            carts.Remove(cart);
        }
        return RedirectToAction("Index");
    }

    public IActionResult Checkout(int[] selectedItems)
    {
        if (selectedItems == null || selectedItems.Length == 0) return RedirectToAction("Index");
        var cartItems = carts.Where(c => selectedItems.Contains(c.ProductId)).ToList();
        var newOrders = cartItems.Select(c => new Order
        {
            ProductId = c.ProductId,
            ProductName = c.ProductName,
            Quantity = c.Quantity,
            ProductPrice = c.ProductPrice,
        }).ToList();
        orders.AddRange([ newOrders ]);
        carts.RemoveAll(c => selectedItems.Contains(c.ProductId));
        return RedirectToAction("Index");
    }
}