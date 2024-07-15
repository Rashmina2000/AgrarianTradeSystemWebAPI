namespace AgrarianTradeSystemWebAPI.Models.UserModels
{
    public class ChangeDetailsDto
    {
        public required string Email { get; set; } = string.Empty;
        public required string FName { get; set; } = string.Empty;
        public required string LName { get; set; } = string.Empty;
        public required string PhoneNumber { get; set; } = string.Empty;
        public required string UserName { get; set; } = string.Empty;
        public required string AddL1 { get; set; } = string.Empty;
        public required string AddL2 { get; set; } = string.Empty;
        public required string AddL3 { get; set; } = string.Empty;
    }
}
