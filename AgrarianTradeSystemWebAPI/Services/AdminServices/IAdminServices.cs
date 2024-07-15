using AgrarianTradeSystemWebAPI.Models.AdminModels;
using AgrarianTradeSystemWebAPI.Models.RefreshToken;
using AgrarianTradeSystemWebAPI.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace AgrarianTradeSystemWebAPI.Services.AdminServices
{
    public interface IAdminServices
    {
        Task<TokenViewModel> Login(LoginDto request);
        Task CreateAdmin(AdminDto request);
        Task<List<GetCourierModel>> GetAllNewCouriers();
        Task<List<GetCourierModel>> GetAllApprovedCouriers();
        Task<List<GetFarmerModel>> GetAllNewFarmers();
        Task<List<GetFarmerModel>> GetAllApprovedFarmers();
        Task<string> ApproveFarmer(string request);
        Task<string> ApproveCourier(string request);
        Task<string> DenyFarmer(UserDenyDto request);
        Task<string> DenyCourier(UserDenyDto request);
    }
}
