namespace AgrarianTradeSystemWebAPI.Dtos
{
	public class AddReviewDto
	{
		public int OrderID { get; set; }
		public string Comment { get; set; } = string.Empty;
		public int SellerRating { get; set; }
		public int DeliverRating { get; set; }
		public int ProductRating { get; set; }
	}
}
