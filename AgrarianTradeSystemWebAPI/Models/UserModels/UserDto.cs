using System.ComponentModel.DataAnnotations;

namespace AgrarianTradeSystemWebAPI.Models.UserModels
{
    public class UserDto
    {
        public required string Username { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public required string First_Name { get; set; } = string.Empty;
        public required string Last_Name { get; set; } = string.Empty;
        [EmailAddress]
        public required string Email { get; set; } = string.Empty;
        [RegularExpression(@"^\d{10}$")]
        public required string Phone { get; set; } = string.Empty;
        [RegularExpression(@"^\d{12}$")]
        public required string NICnumber { get; set; } = string.Empty;
        public required string AddressLine1 { get; set; } = string.Empty;
        public required string AddressLine2 { get; set; } = string.Empty;
        public required string AddressLine3 { get; set; } = string.Empty;
        public string ProfileImg { get; set;} = string.Empty;
    }
}
