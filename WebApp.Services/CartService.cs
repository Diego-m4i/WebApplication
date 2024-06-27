using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services
{
    public class CartService
    {
        private readonly AppDbContext _dbContext;

        public CartService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            return await _dbContext.Carts.Include(c => c.CartItems)
                                         .ThenInclude(ci => ci.Product)
                                         .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddProductToCart(string userId, Product product, int quantity)
        {
            var cart = await GetCartAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                _dbContext.Carts.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == product.ProductId);
            if (cartItem == null)
            {
                cartItem = new CartItem { Product = product, Quantity = quantity };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveProductFromCart(string userId, Guid productId)
        {
            var cart = await GetCartAsync(userId);
            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task EmptyCart(string userId)
        {
            var cart = await GetCartAsync(userId);
            if (cart != null)
            {
                cart.CartItems.Clear();
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> SubmitProducts(string userId)
        {
            var cart = await GetCartAsync(userId);
            if (cart != null && cart.CartItems.Any())
            {
                var orderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    Product = ci.Product,
                    Quantity = ci.Quantity
                }).ToList();

                var order = new Order
                {
                    UserId = userId,
                    OrderItems = orderItems,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending"
                };

                _dbContext.Orders.Add(order);
                cart.CartItems.Clear();
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
