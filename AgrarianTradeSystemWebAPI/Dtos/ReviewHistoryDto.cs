namespace AgrarianTradeSystemWebAPI.Dtos
{
    public class ReviewHistoryDto
    {
        public int ReviewId { get; set; }
        public int OrderID { get; set; }
        public decimal TotalQuantity { get; set; } = 0;
        public string? ProductTitle { get; set; } = string.Empty;
        public string? ProductDescription { get; set; }
        public string? ProductImageUrl { get; set; } = string.Empty;
        public string? ProductType { get; set; } = string.Empty;
        public DateTime OrderedDate { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public string? ReviewImageUrl { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public int SellerRating { get; set; }
        public int DeliverRating { get; set; }
        public int ProductRating { get; set; }
    }
}
