using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public CartController(CartService cartService, ProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IActionResult> IndexCart()
        {
            var userId = User.Identity.Name;
            var cart = await _cartService.GetCartAsync(userId);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid productId)
        {
            var userId = User.Identity.Name;
            var product = await _productService.GetProductByIdAsync(productId);
            if (product != null)
            {
                await _cartService.AddProductToCart(userId, product, 1);
            }
            return RedirectToAction("IndexCart");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(Guid productId)
        {
            var userId = User.Identity.Name;
            await _cartService.RemoveProductFromCart(userId, productId);
            return RedirectToAction("IndexCart");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.Identity.Name;
            await _cartService.SubmitProducts(userId);
            return RedirectToAction("IndexCart");
        }
    }
}