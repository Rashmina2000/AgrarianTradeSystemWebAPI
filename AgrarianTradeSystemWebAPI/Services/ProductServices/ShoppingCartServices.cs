using AgrarianTradeSystemWebAPI.Data;
using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgrarianTradeSystemWebAPI.Services.ProductServices
{
    public class ShoppingCartServices : IShoppingCartServices
    {
        private readonly DataContext _context;

        public ShoppingCartServices(DataContext context)
        {
            _context = context;
        }

        //create a new cart for user
        public async Task CreateCartForUserAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var cart = new Cart
            {
                BuyerId = userId,
                TotalPrice = 0.0m,
                CartItems = new List<CartItem>()
            };

            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();
        }
        //add to cart function
        public async Task<Cart> AddToCart(string buyerId, int productId, int quantity)
        {
            // Check if the buyer exists
            var buyer = await _context.Users.FindAsync(buyerId);
            if (buyer == null)
            {
                throw new InvalidOperationException("Buyer not found.");
            }

            // Check if the product exists
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            // Retrieve the customer's cart (or create a new one if it doesn't exist)
            var cart = await _context.Cart
                                      .Include(c => c.CartItems)
                                      .SingleOrDefaultAsync(c => c.BuyerId == buyerId);

            if (cart == null)
            {
                cart = new Cart { BuyerId = buyerId };
                _context.Cart.Add(cart);
            }

            // Check if the product is already in the cart
            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingCartItem != null)
            {
                // If the product already exists in the cart, update the quantity
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // If the product doesn't exist in the cart, add it as a new cart item
                var cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                cart.CartItems.Add(cartItem);
            }

            // Update the total price of the cart
            cart.TotalPrice = (decimal)cart.CartItems
                .Where(ci => ci.Product != null) // Filter out cart items with null product references
                .Sum(ci => ci.Quantity * ci.Product.UnitPrice);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return cart;
        }

        //get all cart items
        public List<CartItemDto> GetCartItems(string buyerId)
        {
            var cartItems = _context.CartItems
                .Where(ci => ci.Cart.BuyerId == buyerId)
                .Select(ci => new CartItemDto
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductImageUrl = ci.Product.ProductImageUrl,
                    ProductName = ci.Product.ProductTitle,
                    Price = (decimal)ci.Product.UnitPrice,
                    Quantity = ci.Quantity
                })
                .ToList();

            return cartItems;
        }

         //delete cart items
        public List<CartItemDto> DeleteCartItem(string buyerId, int cartItemId)
        {
            // Find the cart item to delete
            var cartItem = _context.CartItems
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart.BuyerId == buyerId && ci.CartItemId == cartItemId)
                .FirstOrDefault();
            if (cartItem == null)
            {

                return null;
            }
            // Remove the cart item from the context
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            // After deleting the cart item, retrieve the updated list of cart items
            var updatedCartItems = _context.CartItems
                .Where(ci => ci.Cart.BuyerId == buyerId)
                .Select(ci => new CartItemDto
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductImageUrl = ci.Product.ProductImageUrl,
                    ProductName = ci.Product.ProductTitle,
                    Price = (decimal)ci.Product.UnitPrice,
                    Quantity = ci.Quantity
                })
                .ToList();

            return updatedCartItems;
        }
    }
}

