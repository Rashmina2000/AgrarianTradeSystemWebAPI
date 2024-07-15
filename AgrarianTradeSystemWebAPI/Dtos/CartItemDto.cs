namespace AgrarianTradeSystemWebAPI.Dtos
{
	public class CartItemDto
	{
		public int CartItemId { get; set; }
		public int ProductId { get; set; }
		public string? ProductName { get; set; }
		public string? ProductImageUrl { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
