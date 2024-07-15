using System.ComponentModel.DataAnnotations;

namespace AgrarianTradeSystemWebAPI.Dtos
{
    public class CourierOrderDto
    {
        [Key]
        public int OrderID { get; set; }
        public string? FarmerID { get; set; }
        public  string? CourierID { get; set; }
        public  string? BuyerID { get; set; }
        public string? ProductTitle { get; set; } = string.Empty;
        public string? ProductImageUrl { get; set; } = string.Empty;
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime OrderedDate { get; set; }=DateTime.Now;
        public decimal DeliveryFee { get; set; }
        public decimal TotalQuantity { get; set; }
        public string? OrderStatus { get; set; }
        public string? CustomerFName { get; set; } = string.Empty;
        public string? CustomerLName { get; set; } = string.Empty;
        public string? CustomerAddL1 { get; set; } = string.Empty;
        public string? CustomerAddL2 { get; set; } = string.Empty;
        public string? CustomerAddL3 { get; set; } = string.Empty;
        public string? CustomerPhoneNumber { get; set; } = string.Empty;
        public string? FarmerFName { get; set; } = string.Empty;
        public string? FarmerLName { get; set; } = string.Empty;
        public string? FarmerAddL1 { get; set; } = string.Empty;
        public string? FarmerAddL2 { get; set; } = string.Empty;
        public string? FarmerAddL3 { get; set; } = string.Empty;
        public string? FarmerPhoneNumber { get; set; } = string.Empty;
   
    }
}
