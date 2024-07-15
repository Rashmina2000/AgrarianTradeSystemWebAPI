namespace AgrarianTradeSystemWebAPI.Models.UserModels
{
    public class CourierDto : UserDto
    {
        public required string VehicleNumber { get; set; } = string.Empty;
        public string VehicleImg { get; set; } = string.Empty;
        public string LicenseImg { get; set; } = string.Empty;
    }
}
