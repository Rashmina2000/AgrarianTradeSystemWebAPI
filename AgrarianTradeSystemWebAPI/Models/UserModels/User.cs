using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AgrarianTradeSystemWebAPI.Models.UserModels
{
    public class User
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [Key] 
        public string Email { get; set; } = string.Empty;
        public bool EmailVerified { get; set; } = false;
        [Required]
        public string NIC { get; set; } = string.Empty;
        [Required]
        public string AddL1 { get; set; } = string.Empty;
        [Required]
        public string AddL2 { get; set; } = string.Empty;
        [Required]
        public string AddL3 { get; set;} = string.Empty;
        public string ProfileImg { get; set; } = string.Empty;
        public string VerificationToken { get; set; } = string.Empty;
        public DateTime VerifiedAt { get; set; }
        public string PasswordResetToken { get; set; } = string.Empty;
        public DateTime? ResetTokenExpireAt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }

		[JsonIgnore]
		public Cart? Cart { get; set; }
	}
}
