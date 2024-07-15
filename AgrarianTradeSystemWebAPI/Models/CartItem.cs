using System.Text.Json.Serialization;

namespace AgrarianTradeSystemWebAPI.Models
{
	public class CartItem
	{
		public int CartItemId { get; set; }
		public int CartId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }

		[JsonIgnore]
		public Product? Product { get; set; }
		[JsonIgnore]
		public Cart? Cart { get; set; }

	}
}
