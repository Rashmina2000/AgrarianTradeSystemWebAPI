namespace AgrarianTradeSystemWebAPI.Models.UserModels
{
    public class FarmerDto : UserDto
    {
        public required string CropDetails { get; set; } = string.Empty;
        public string NICFrontImg { get; set; } = string.Empty;
        public string NICBackImg { get; set; } = string.Empty;
        public string GNCImage { get; set; } = string.Empty;
    }
}
