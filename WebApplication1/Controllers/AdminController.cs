    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ProductService _productService;
        private readonly OrderService _orderService;

        public AdminController(ProductService productService, OrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        public async Task<IActionResult> ManageProducts()
        {
            var products = await _productService.GetProductsAsync();
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction("ManageProducts");
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction("ManageProducts");
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("ManageProducts");
        }

        public async Task<IActionResult> ManageOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, string status)
        {
            await _orderService.UpdateOrderStatus(id, status);
            return RedirectToAction("ManageOrders");
        }
    }
}
