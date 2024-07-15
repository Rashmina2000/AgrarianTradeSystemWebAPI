using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using AgrarianTradeSystemWebAPI.Services.ProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgrarianTradeSystemWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ShoppingCartController : ControllerBase
	{
		private readonly IShoppingCartServices _shoppingCartServices;

        public ShoppingCartController(IShoppingCartServices shoppingCartServices)
        {
			_shoppingCartServices=shoppingCartServices;

		}

		[HttpPost("add-to-cart")]
		public async Task<IActionResult> AddToCart([FromBody] AddToCartRequestDto request)
		{
			try
			{
				var cart = await _shoppingCartServices.AddToCart(request.BuyerId, request.ProductId, request.Quantity);
				return Ok(cart);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				// Return a custom error message instead of serializing the entire exception object
				return BadRequest("An error occurred while processing the request.");
			}
		}



		[HttpGet("items")]
		public IActionResult GetCartItems(string customerId)
		{
			try
			{
				var cartItems = _shoppingCartServices.GetCartItems(customerId);
				return Ok(cartItems);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while retrieving cart items: {ex.Message}");
			}
		}

		[HttpDelete("delete-cart-item")]
		public IActionResult DeleteCartItem(string buyerId, int cartItemId)
		{
			try
			{
				var updatedCartItems = _shoppingCartServices.DeleteCartItem(buyerId, cartItemId);
				if (updatedCartItems == null)
				{
					return NotFound(); // Cart item not found
				}
				return Ok(updatedCartItems); // Return the updated list of cart items
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while deleting the cart item: {ex.Message}");
			}
		}



	}
}
