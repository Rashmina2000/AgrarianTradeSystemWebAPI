namespace AgrarianTradeSystemWebAPI.Dtos
{
    public class ReturnDto
    {
        public int ReturnId { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderedDate { get; set; } = DateTime.Now;
        public string? ProductTitle { get; set; } = string.Empty;
      
        public string? ProductDescription { get; set; }
        public string? ProductImageUrl { get; set; } = string.Empty;
        public string? ProductType { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal ReturnQuantity { get; set; }
        public decimal ReturnPrice { get; set; } = 0;
        public string? SellerName { get; set; }
        public string? Reason { get; set; } = string.Empty;
        public string? ReturnImageUrl { get; set; } = string.Empty;
        public DateTime ReturnDate { get; set; } = DateTime.Now;
       
    }
}
