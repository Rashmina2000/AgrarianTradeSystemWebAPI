using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AgrarianTradeSystemWebAPI.Models
{
	public class Returns
	{
		[Key]
        public int ReturnID { get; set; }
		public int OrderID { get; set; }
		public required string Reason { get; set; }
		public DateTime ReturnDate { get; set;}
        public decimal ReturnQuantity { get; set; }
		public decimal ReturnPrice { get; set; } = 0;
        public string? ReturnImageUrl { get; set; }
		[JsonIgnore]
		public Orders? Order{ get; set;}

    }
}
