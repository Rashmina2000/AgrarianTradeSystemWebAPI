using AgrarianTradeSystemWebAPI.Models.UserModels;
using System.Text.Json.Serialization;

namespace AgrarianTradeSystemWebAPI.Models
{
	public class Cart
	{
		public int CartId { get; set; }
		public string? BuyerId { get; set; }
		public decimal TotalPrice { get; set; }
		[JsonIgnore]
		public ICollection<CartItem>? CartItems { get; set; }
		[JsonIgnore]
		public User? Buyer { get; set; }

	}
}
