using AgrarianTradeSystemWebAPI.Data;
using AgrarianTradeSystemWebAPI.Models;
using AgrarianTradeSystemWebAPI.Models.AdminModels;
using AgrarianTradeSystemWebAPI.Models.RefreshToken;
using AgrarianTradeSystemWebAPI.Models.UserModels;
using AgrarianTradeSystemWebAPI.Services.EmailService;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgrarianTradeSystemWebAPI.Services.AdminServices
{
    public class AdminServices : IAdminServices
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailService;
        public readonly IConfiguration _configuration;
        public AdminServices(DataContext context, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }

        public static Admin admin = new Admin();

        // Service for create new admin --------------------------------
        public async Task CreateAdmin(AdminDto request)
        {
            var existingAdmin = await _context.Admins.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingAdmin != null)
            {
                throw new EmailException("Email exist");
            }
            string PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            admin.Email = request.Email;
            admin.Password = PasswordHash;
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
        }

        // Service for admin login --------------------------------------
        public async Task<TokenViewModel> Login(LoginDto request)
        {
            TokenViewModel _TokenViewModel = new();
            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (admin != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Password, admin.Password))
                {
                    throw new LoginException("Email or password is incorrect");
                }
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim(ClaimTypes.Role, "ATSAdmin"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                _TokenViewModel.AccessToken = GenerateToken(authClaims);
            }
            else
            {
                throw new LoginException("Email or password is incorrect");
            }
            return _TokenViewModel;
        }

        // Service for getting all new couriers --------------------------------
        public async Task<List<GetCourierModel>> GetAllNewCouriers()
        {
            var newCourierModels = new List<GetCourierModel>();
            var NewCouriers = await _context.Couriers.Where(courier => !courier.Approved).ToListAsync();
            foreach (var courier in NewCouriers)
            {
                GetCourierModel getCourierModel = new GetCourierModel
                {
                    UserName = courier.Username,
                    FirstName = courier.FirstName,
                    LastName = courier.LastName,
                    Email = courier.Email,
                    PhoneNumber = courier.PhoneNumber,
                    NIC = courier.NIC,
                    AddL1 = courier.AddL1,
                    AddL2 = courier.AddL2,
                    AddL3 = courier.AddL3,
                    ProfileImg = courier.ProfileImg,
                    VehicleNo = courier.VehicleNo,
                    VehicleImg = courier.VehicleImg,
                    LicenseImg = courier.LicenseImg,
                };
                newCourierModels.Add(getCourierModel);
            }
            return newCourierModels;
        }

        // Service for getting all approved couriers ------------------------------
        public async Task<List<GetCourierModel>> GetAllApprovedCouriers()
        {
            var newCourierModels = new List<GetCourierModel>();
            var NewCouriers = await _context.Couriers.Where(courier => courier.Approved).ToListAsync();
            foreach (var courier in NewCouriers)
            {
                GetCourierModel getCourierModel = new GetCourierModel
                {
                    UserName = courier.Username,
                    FirstName = courier.FirstName,
                    LastName = courier.LastName,
                    Email = courier.Email,
                    PhoneNumber = courier.PhoneNumber,
                    NIC = courier.NIC,
                    AddL1 = courier.AddL1,
                    AddL2 = courier.AddL2,
                    AddL3 = courier.AddL3,
                    ProfileImg = courier.ProfileImg,
                    VehicleNo = courier.VehicleNo,
                    VehicleImg = courier.VehicleImg,
                    LicenseImg = courier.LicenseImg,
                };
                newCourierModels.Add(getCourierModel);
            }
            return newCourierModels;
        }

        // Service for getting all new farmers -----------------------------------------
        public async Task<List<GetFarmerModel>> GetAllNewFarmers()
        {
            var newFarmerModels = new List<GetFarmerModel>();
            var newFarmers = await _context.Farmers.Where(farmer => !farmer.Approved).ToListAsync();
            foreach(var farmer in newFarmers)
            {
                GetFarmerModel getFarmerModel = new GetFarmerModel
                {
                    UserName = farmer.Username,
                    FirstName = farmer.FirstName,
                    LastName = farmer.LastName,
                    Email = farmer.Email,
                    PhoneNumber = farmer.PhoneNumber,
                    NIC = farmer.NIC,
                    AddL1 = farmer.AddL1,
                    AddL2 = farmer.AddL2,
                    AddL3 = farmer.AddL3,
                    ProfileImg = farmer.ProfileImg,
                    CropTypes = farmer.CropDetails,
                    GNCertificate = farmer.GSLetterImg,
                    NICFront = farmer.NICFrontImg,
                    NICBack = farmer.NICBackImg,
                };
                newFarmerModels.Add(getFarmerModel);

            }
            return newFarmerModels;
        }

        // Service for getting all approved farmers --------------------------------------------
        public async Task<List<GetFarmerModel>> GetAllApprovedFarmers()
        {
            var newFarmerModels = new List<GetFarmerModel>();
            var newFarmers = await _context.Farmers.Where(farmer => farmer.Approved).ToListAsync();
            foreach (var farmer in newFarmers)
            {
                GetFarmerModel getFarmerModel = new GetFarmerModel
                {
                    UserName = farmer.Username,
                    FirstName = farmer.FirstName,
                    LastName = farmer.LastName,
                    Email = farmer.Email,
                    PhoneNumber = farmer.PhoneNumber,
                    NIC = farmer.NIC,
                    AddL1 = farmer.AddL1,
                    AddL2 = farmer.AddL2,
                    AddL3 = farmer.AddL3,
                    ProfileImg = farmer.ProfileImg,
                    CropTypes = farmer.CropDetails,
                    GNCertificate = farmer.GSLetterImg,
                    NICFront = farmer.NICFrontImg,
                    NICBack = farmer.NICBackImg,
                };
                newFarmerModels.Add(getFarmerModel);

            }
            return newFarmerModels;
        }

        // Service for approve a courier --------------------------------------------
        public async Task<string> ApproveCourier(string request)
        {
            var courier = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request);
            if (courier == null)
            {
                throw new AdminErrorException("Invalid Email");
            }
            _emailService.approveUserMail(request, courier.FirstName, courier.LastName);
            courier.Approved = true;
            await _context.SaveChangesAsync();
            return ("Approved successfully");
        }

        // Service for approve a farmer ----------------------------------------------
        public async Task<string> ApproveFarmer(string request)
        {
            var farmer = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request);
            if (farmer == null)
            {
                throw new AdminErrorException("Invalid Email");
            }
            _emailService.approveUserMail(request, farmer.FirstName, farmer.LastName);
            farmer.Approved = true;
            await _context.SaveChangesAsync();
            return ("Approved successfully");
        }

        // Service for reject a farmer --------------------------------------------
        public async Task<string> DenyFarmer(UserDenyDto request)
        {
            var farmer = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (farmer == null)
            {
                throw new AdminErrorException("Invalid Email");
            }
            _emailService.rejectUserMail(request.Email, farmer.FirstName, farmer.LastName, request.Reason);
            _context.Farmers.Remove(farmer);
            await _context.SaveChangesAsync();
            return ("Farmer denied");
        }

        // Service for reject a courier --------------------------------------------
        public async Task<string> DenyCourier(UserDenyDto request)
        {
            var courier = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (courier == null)
            {
                throw new AdminErrorException("Invalid Email");
            }
            _emailService.rejectUserMail(request.Email, courier.FirstName, courier.LastName, request.Reason);
            _context.Couriers.Remove(courier);
            await _context.SaveChangesAsync();
            return ("Courier denied");
        }

        // Service for generate JWT token for admin login ------------------------------------------
        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTKey:SecretKey").Value!));
            var TokenExpireTime = Convert.ToInt64(_configuration.GetSection("JWTKey:TokenExpiryTimeInHour").Value!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(TokenExpireTime),
                SigningCredentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
