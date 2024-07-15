using System.ComponentModel.DataAnnotations;

namespace AgrarianTradeSystemWebAPI.Dtos
{
	public class CourierListDto
	{
	
        public required string CourierID { get; set; }
		public string? CourierFName { get; set; }
		public string? CourierLName { get; set; }
		public string ?CourierImageUrl { get; set; }

		public string? AddressLine1 { get; set; }
		public string? AddressLine2 { get; set; }
		public string? AddressLine3 { get; set; }


	}
}
