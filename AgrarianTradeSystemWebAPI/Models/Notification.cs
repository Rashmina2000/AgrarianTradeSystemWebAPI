using System.ComponentModel.DataAnnotations;

namespace AgrarianTradeSystemWebAPI.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public int OrderID { get; set; }
        public string? OrderStatus { get; set; }
        public string? Message { get; set; }
        public DateTime? SendAt { get; set; } = DateTime.Now;
        public bool? ISSeen { get; set; }
    }
}
