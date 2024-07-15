namespace AgrarianTradeSystemWebAPI.Dtos
{
    public class ReturnOrderDto
    {
        public int OrderID { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalQuantity { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductType{ get; set; }
        public string? SellerName{ get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImageUrl { get; set; }
    }
}


   

