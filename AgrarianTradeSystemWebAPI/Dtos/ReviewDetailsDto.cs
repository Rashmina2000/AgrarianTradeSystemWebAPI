namespace AgrarianTradeSystemWebAPI.Dtos
{
    public class ReviewDetailsDto
    {
        public int ReviewId { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderedDate { get; set; } = DateTime.Now;
        public string? ProductTitle { get; set; } = string.Empty;
        public string? ProductDescription { get; set; }
        public string? ProductImageUrl { get; set; } = string.Empty;
        public string? ProductType { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalQuantity { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string ReviewImageUrl { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public int SellerRating { get; set; }
        public int DeliverRating { get; set; }
        public int ProductRating { get; set; }
        public string? Reply { get; set; }
        public string BuyerFName { get; set; } = string.Empty;
        public string BuyerLName { get; set; } = string.Empty;
        public string BuyerImageUrl { get; set; } = string.Empty;
        public int productId { get; set; }





    }
}
