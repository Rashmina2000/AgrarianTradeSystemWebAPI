using System.ComponentModel.DataAnnotations;

namespace AgrarianTradeSystemWebAPI.Dtos
{
    public class BuyerOrderDto
    {
        [Key]
        public int OrderID { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductImageUrl { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string ProductDescription { get; set; }= string.Empty;
        public DateTime? DeliveryDate { get; set; }
        public DateTime OrderedDate { get; set; }
        public decimal DeliveryFee { get; set; }
        public string? OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalQuantity { get; set; }
        public string? FarmerFName { get; set; } = string.Empty;
        public string? FarmerLName { get; set; } = string.Empty;
        public string? FarmerAddL1 { get; set; } = string.Empty;
        public string? FarmerAddL2 { get; set; } = string.Empty;
        public string? FarmerAddL3 { get; set; } = string.Empty;
        public string? FarmerPhoneNumber { get; set; } = string.Empty;
        public string? CourierFName { get; set; } = string.Empty;
        public string? CourierLName { get; set; } = string.Empty;
        public string? CourierAddL1 { get; set; } = string.Empty;
        public string? CourierAddL2 { get; set; } = string.Empty;
        public string? CourierAddL3 { get; set; } = string.Empty;
        public string? CourierPhoneNumber { get; set; } = string.Empty;



    }
}
