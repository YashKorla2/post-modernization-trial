using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Modernization8.Models;
using Modernization8.Constants;

namespace Modernization8.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    List<Product> products = ProductConstants.Products;
    List<Cart> carts = CartConstants.Carts;

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(string searchTerm)
    {
        var cartItemCount = carts.Count();
        var filteredProducts = string.IsNullOrEmpty(searchTerm) 
            ? products
            : products.Where(
                p => p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                     p.ProductDescription.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                     p.ProductCategory.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        var ViewModel = new ProductViewModel{
            Products = filteredProducts,
            CartItemCount = cartItemCount
        };

        return View(ViewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            product.ProductId = products.Count + 1;
            products.Add(product);
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    public IActionResult Edit(int id)
    {
        var product = products.FirstOrDefault(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            var originalProduct = products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (originalProduct != null)
            {
                originalProduct.ProductName = product.ProductName;
                originalProduct.ProductDescription = product.ProductDescription;
                originalProduct.ProductCategory = product.ProductCategory;
                originalProduct.ProductRating = product.ProductRating;
                originalProduct.ProductPrice = product.ProductPrice;
                originalProduct.ProductAvailableQuantity = product.ProductAvailableQuantity;
                originalProduct.ProductDiscount = product.ProductDiscount;
            }
            else
            {
                return View(product);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    public IActionResult Details(int id)
    {
        var product = products.FirstOrDefault(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpGet]
    [ActionName("Delete")]
    public IActionResult DeleteGet(int id)
    {
        var product = products.FirstOrDefault(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var product = products.Find(p => p.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }

        products.Remove(product);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity = 1)
    {
        var product = products.FirstOrDefault(p => p.ProductId == productId);
        if (product != null)
        {
            carts.Add(new() 
            { 
                ProductId = productId, 
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                Quantity = quantity 
            });
        }
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
