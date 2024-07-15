namespace AgrarianTradeSystemWebAPI.Dtos
{
	public class ProductListDto
	{
		public int ProductId { get; set; }
		public string ProductTitle { get; set; } = string.Empty;
		public  string? FarmerFName { get; set; }
		public string? FarmerLName { get; set; }
		public string? FarmerProfileUrl { get; set; }
		public string? ProductImageUrl { get; set; } = string.Empty;
		public string? FarmerAddL1 { get; set; }
		public string? FarmerAddL2 { get; set; }

		public string? FarmerAddL3 { get; set; }

		public string? FarmerContact { get; set; }

		public string ProductDescription { get; set; } = string.Empty;

		public string ProductType { get; set; } = string.Empty;

		public string Category { get; set; } = string.Empty;

		public double UnitPrice { get; set; }

		public int AvailableStock { get; set; }

		public int MinimumQuantity { get; set; }
	}
}
