namespace AgrarianTradeSystemWebAPI.Models.UserModels
{
    public class ChangePwdDto
    {
        public required string Email { get; set; } = string.Empty;
        public required string NewPassword { get; set; } = string.Empty;
    }
}
