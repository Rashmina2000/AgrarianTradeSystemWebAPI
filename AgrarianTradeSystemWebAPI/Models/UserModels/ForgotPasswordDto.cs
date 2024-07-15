using System.ComponentModel.DataAnnotations;

namespace AgrarianTradeSystemWebAPI.Models.UserModels
{
    public class ForgotPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
