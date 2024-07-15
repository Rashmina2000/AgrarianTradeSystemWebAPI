namespace AgrarianTradeSystemWebAPI.Dtos
{
    public class FarmerOrderDto
    {
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductImageUrl { get; set; } = string.Empty;
        public int OrderID { get; set; }
        public DateTime OrderedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalQuantity { get; set; }
        public string? OrderStatus { get; set; }
        public string? CourierFName { get; set; } = string.Empty;
        public string? CourierLName { get; set; } = string.Empty;
        public string? CourierAddL1 { get; set; } = string.Empty;
        public string? CourierAddL2 { get; set; } = string.Empty;
        public string? CourierAddL3 { get; set; } = string.Empty;
        public string? CourierPhoneNumber { get; set; } = string.Empty;
        public string? CustomerFName { get; set; } = string.Empty;
        public string? CustomerLName { get; set; } = string.Empty;
        public string? CustomerAddL1 { get; set; } = string.Empty;
        public string? CustomerAddL2 { get; set; } = string.Empty;
        public string? CustomerAddL3 { get; set; } = string.Empty;
        public string? CustomerPhoneNumber { get; set; } = string.Empty;
    }
}
