namespace AgrarianTradeSystemWebAPI.Dtos
{
	public class ProductDto
	{
		public string ProductTitle { get; set; } = string.Empty;

		public required string FarmerID { get; set; }

		public string? ProductDescription { get; set; } = string.Empty;

		public string? ProductType { get; set; } = string.Empty;

		public string? Category { get; set; } = string.Empty;

		public double UnitPrice { get; set; }

		public int AvailableStock { get; set; }

		public int MinimumQuantity { get; set; }

	}
}
