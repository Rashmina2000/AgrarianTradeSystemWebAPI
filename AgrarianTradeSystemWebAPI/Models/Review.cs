using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgrarianTradeSystemWebAPI.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int OrderID { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string ReviewImageUrl { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public int SellerRating { get; set; }
        public int DeliverRating { get; set; }
        public int ProductRating { get; set; }
        public string? Reply { get; set; }
        [JsonIgnore]
        public Orders? Order { get; set; }
    }
}
