using AgrarianTradeSystemWebAPI.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AgrarianTradeSystemWebAPI.Models
{
	public class Orders
	{
        [Key]
		public int OrderID { get; set; }
		public string? BuyerID { get; set; }
        public int ProductID { get; set; }
        public string? DeliveryAddressLine1 { get; set; }
		public string? DeliveryAddressLine2 { get; set; }
		public string? DeliveryAddressLine3 { get; set; }
		public string? OrderStatus { get; set; }
		public decimal DeliveryFee { get; set; }
		public DateTime OrderedDate { get; set; }
		public decimal TotalPrice { get; set; }
		public decimal TotalQuantity { get; set; }
		public string? CourierID { get; set; }
		public DateTime? PickupDate { get; set; }
		public DateTime? DeliveryDate { get; set; }	
		[JsonIgnore]
		public User? Buyer { get; set; }
		[JsonIgnore]
		public Product? Product { get; set; }
		[JsonIgnore]
		public Courier? Courier { get; set; }

	}
}
