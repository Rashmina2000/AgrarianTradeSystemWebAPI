using System.ComponentModel.DataAnnotations;

namespace AgrarianTradeSystemWebAPI.Models.UserModels
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
