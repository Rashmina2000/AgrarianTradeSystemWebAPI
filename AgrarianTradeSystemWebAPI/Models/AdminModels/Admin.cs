using System.ComponentModel.DataAnnotations;

namespace AgrarianTradeSystemWebAPI.Models.AdminModels
{
    public class Admin
    {
        [Key]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
