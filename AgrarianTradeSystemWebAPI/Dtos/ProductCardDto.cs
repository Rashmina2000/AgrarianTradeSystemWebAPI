namespace AgrarianTradeSystemWebAPI.Dtos
{
	public class ProductCardDto
	{
		public int ProductID { get; set; }

		public string? FarmerAddL3 { get; set; }
		public string? ProductTitle { get; set; } = string.Empty;

		public string? ProductImageUrl { get; set; } = string.Empty;

		public string? ProductType { get; set; } = string.Empty;

		public string? Category { get; set; } = string.Empty;

		public double UnitPrice { get; set; }

		public int AvailableStock { get; set; }

		public int MinimumQuantity { get; set; }
	}
}
