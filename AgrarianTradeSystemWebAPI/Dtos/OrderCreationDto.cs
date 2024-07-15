namespace AgrarianTradeSystemWebAPI.Dtos
{
	public class OrderCreationDto
	{
		public string? BuyerID { get; set; }
		public int ProductID { get; set; }
		public string? DeliveryAddressLine1 { get; set; }
		public string? DeliveryAddressLine2 { get; set; }
		public string ?DeliveryAddressLine3 { get; set; }
		public string? OrderStatus { get; set; }
		public decimal DeliveryFee { get; set; }
        public decimal TotalQuantity { get; set; }
        public DateTime OrderedDate { get; set; }= DateTime.Now;
		public decimal TotalPrice { get; set; }
		public string? CourierID { get; set; }
		public DateTime? PickupDate { get; set; }
		public DateTime? DeliveryDate { get; set; }
	}

}
