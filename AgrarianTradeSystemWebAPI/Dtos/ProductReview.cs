namespace AgrarianTradeSystemWebAPI.Dtos
{
    public class ProductReviewDto
    {
        public int ReviewId { get; set; }
        public int OrderID { get; set; }
        public string? BuyerFirstName { get; set; }
        public string? BuyerLastName { get; set; }
        public string? BuyerProfileImageUrl { get; set; }
        public string? SellerFirstName { get; set; }
        public string? SellerLastName { get; set; }
        public string? SellerProfileImageUrl { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public string? ReviewImageUrl { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public int SellerRating { get; set; }
        public int DeliverRating { get; set; }
        public int ProductRating { get; set; }
        public string? Reply { get; set; }
    }
}
