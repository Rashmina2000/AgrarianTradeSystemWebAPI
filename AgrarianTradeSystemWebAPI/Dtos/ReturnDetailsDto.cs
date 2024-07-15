namespace AgrarianTradeSystemWebAPI.Dtos
{
    public class ReturnDetailsDto
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
        public string? SellerName { get; set; }
        public string? Reason { get; set; } = string.Empty;
        public string? ReturnImageUrl { get; set; } = string.Empty;
        public decimal ReturnQuantity { get; set; }
        public decimal ReturnPrice { get; set; } = 0;
        public DateTime ReturnDate { get; set; } = DateTime.Now;
        public string BuyerFName { get; set; } = string.Empty;
        public string BuyerLName { get; set; } = string.Empty;
        public string BuyerPNumber { get; set; } = string.Empty;
        public string BuyerAddL1 { get; set; } = string.Empty;
        public string BuyerAddL2 { get; set; } = string.Empty;
        public string BuyerAddL3 { get; set; } = string.Empty;

    }
}


