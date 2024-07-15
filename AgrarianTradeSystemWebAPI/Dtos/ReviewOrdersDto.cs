namespace AgrarianTradeSystemWebAPI.Dtos
{
	public class ReviewOrdersDto
	{
        public int OrderID { get; set; }
        public decimal TotalQuantity { get; set; } = 0;
        public DateTime OrderedDate { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string?ProductDescription { get; set; }
        public string? ProductImageUrl { get; set; }
        public string? ProductType { get; set; }


    }
}
