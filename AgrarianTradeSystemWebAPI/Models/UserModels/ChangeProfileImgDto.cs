namespace AgrarianTradeSystemWebAPI.Models.UserModels
{
    public class ChangeProfileImgDto
    {
        public required string Email { get; set; } = string.Empty;
        public string ProfileImg { get; set; } = string.Empty;
    }
}
