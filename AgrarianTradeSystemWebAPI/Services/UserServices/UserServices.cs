using AgrarianTradeSystemWebAPI.Data;
using AgrarianTradeSystemWebAPI.Models.RefreshToken;
using AgrarianTradeSystemWebAPI.Models.UserModels;
using AgrarianTradeSystemWebAPI.Services.EmailService;
using AgrarianTradeSystemWebAPI.Services.ProductServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AgrarianTradeSystemWebAPI.Services.UserServices
{
    public class UserServices : IUserServices
    {
        public readonly DataContext _context;
        public readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private const string AzureContainerProfileImg = "profilepic";
        private const string AzureContainerNICImg = "nicimage";
        private const string AzureContainerVehicleImg = "vehicleimage";
        private const string AzureContainerGNSImg = "gramaniladhari";
        private readonly IFileServices _fileServices;
        private readonly IShoppingCartServices _shoppingCartServices;
        public UserServices(DataContext context, IConfiguration configuration, IEmailService emailService, IFileServices fileServices, IShoppingCartServices shoppingCartServices)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _fileServices = fileServices;
            _shoppingCartServices = shoppingCartServices;

        }

        public static User user = new User();
        public static Farmer farmer = new Farmer();
        public static Courier courier = new Courier();

        // User registration service ----------------------------------------
        public async Task UserRegister(UserDto request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            var existingFarmer = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request.Email);
            var existingCourier = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null || existingFarmer != null || existingCourier != null)
            {
                throw new EmailException("Email exist");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.FirstName = request.First_Name;
            user.LastName = request.Last_Name;
            user.Email = request.Email;
            user.PhoneNumber = request.Phone;
            user.NIC = request.NICnumber;
            user.AddL1 = request.AddressLine1;
            user.AddL2 = request.AddressLine2;
            user.AddL3 = request.AddressLine3;
            user.ProfileImg = request.ProfileImg;
            user.VerificationToken = CreateCustomToken();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _emailService.SendUserRegisterEmail(user.Email, user.FirstName, user.LastName, user.VerificationToken);
            await _shoppingCartServices.CreateCartForUserAsync(user.Email);
        }

        // Farmer registration service -----------------------------------
        public async Task FarmerRegister(FarmerDto request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            var existingFarmer = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request.Email);
            var existingCourier = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null || existingFarmer != null || existingCourier != null)
            {
                throw new EmailException("Email exist");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            farmer.Username = request.Username;
            farmer.PasswordHash = passwordHash;
            farmer.FirstName = request.First_Name;
            farmer.LastName = request.Last_Name;
            farmer.Email = request.Email;
            farmer.PhoneNumber = request.Phone;
            farmer.NIC = request.NICnumber;
            farmer.AddL1 = request.AddressLine1;
            farmer.AddL2 = request.AddressLine2;
            farmer.AddL3 = request.AddressLine3;
            farmer.CropDetails = request.CropDetails;
            farmer.ProfileImg = request.ProfileImg;
            farmer.NICFrontImg = request.NICFrontImg;
            farmer.NICBackImg = request.NICBackImg;
            farmer.GSLetterImg = request.GNCImage;
            farmer.VerificationToken = CreateCustomToken();

            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();
            _emailService.SendRegisterEmail(farmer.Email, farmer.FirstName, farmer.LastName, farmer.VerificationToken);
        }

        // Courier registration service -------------------------------------
        public async Task CourierRegister(CourierDto request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            var existingFarmer = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request.Email);
            var existingCourier = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null || existingFarmer != null || existingCourier != null)
            {
                throw new EmailException("Email exist");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            courier.Username = request.Username;
            courier.PasswordHash = passwordHash;
            courier.FirstName = request.First_Name;
            courier.LastName = request.Last_Name;
            courier.Email = request.Email;
            courier.PhoneNumber = request.Phone;
            courier.NIC = request.NICnumber;
            courier.AddL1 = request.AddressLine1;
            courier.AddL2 = request.AddressLine2;
            courier.AddL3 = request.AddressLine3;
            courier.VehicleNo = request.VehicleNumber;
            courier.ProfileImg = request.ProfileImg;
            courier.VehicleImg = request.VehicleImg;
            courier.LicenseImg = request.LicenseImg;
            courier.VerificationToken = CreateCustomToken();

            _context.Couriers.Add(courier);
            await _context.SaveChangesAsync();
            _emailService.SendRegisterEmail(courier.Email, courier.FirstName, courier.LastName, courier.VerificationToken);
        }

        // Login service ----------------------------------------------
        public async Task<TokenViewModel> Login(LoginDto request)
        {
            TokenViewModel _TokenViewModel = new();
            var loginUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginFarmerUser = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginCourierUser = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (loginUser != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Password, loginUser.PasswordHash))
                {
                    throw new LoginException("Email or password is incorrect");
                }
                if (loginUser.EmailVerified == false)
                {
                    throw new LoginException("Not verified");
                }
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "User")
                };
                _TokenViewModel.AccessToken = GenerateToken(authClaims);
                var _RefreshTokenValidityInDays = Convert.ToInt64(_configuration.GetSection("RefreshTokenValidityInDays").Value!);
                loginUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(_RefreshTokenValidityInDays);
                await _context.SaveChangesAsync();
            }
            else if (loginFarmerUser != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Password, loginFarmerUser.PasswordHash))
                {
                    throw new LoginException("Email or password is incorrect");
                }
                if (loginFarmerUser.EmailVerified == false)
                {
                    throw new LoginException("Not verified");
                }
                if (loginFarmerUser.Approved == false)
                {
                    throw new LoginException("Not approved");
                }
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginFarmerUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "Farmer")
                };
                _TokenViewModel.AccessToken = GenerateToken(authClaims);
                var _RefreshTokenValidityInDays = Convert.ToInt64(_configuration.GetSection("RefreshTokenValidityInDays").Value!);
                loginFarmerUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(_RefreshTokenValidityInDays);
                await _context.SaveChangesAsync();
            }
            else if (loginCourierUser != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Password, loginCourierUser.PasswordHash))
                {
                    throw new LoginException("Email or password is incorrect");
                }
                if (loginCourierUser.EmailVerified == false)
                {
                    throw new LoginException("Not verified");
                }
                if (loginCourierUser.Approved == false)
                {
                    throw new LoginException("Not approved");
                }
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginCourierUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "Courier")
                };
                _TokenViewModel.AccessToken = GenerateToken(authClaims);
                var _RefreshTokenValidityInDays = Convert.ToInt64(_configuration.GetSection("RefreshTokenValidityInDays").Value!);
                loginCourierUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(_RefreshTokenValidityInDays);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new LoginException("Email or password is incorrect");
            }
            return _TokenViewModel;
        }

        // Getting verification link through email --------------------------------
        public async Task<string> GetVerifyLink(GetVerifyLinkDto request)
        {
            var loginuser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginFarmeruser = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginCourieruser = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (loginuser != null)
            {
                var token = loginuser.VerificationToken;
                _emailService.verifyEmail(loginuser.Email, token);
            }
            else if (loginFarmeruser != null)
            {
                var token = loginFarmeruser.VerificationToken;
                _emailService.verifyEmail(loginFarmeruser.Email, token);
            }
            else if (loginCourieruser != null)
            {
                var token = loginCourieruser.VerificationToken;
                _emailService.verifyEmail(loginCourieruser.Email, token);
            }
            else
            {
                throw new Exception("Invalid Email");
            }
            return ("Email is sent");
        }

        // Verifing the email --------------------------------------------
        public async Task<string> Verify(VerifyDto request)
        {
            var loginuser = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == request.token);
            var loginFarmeruser = await _context.Farmers.FirstOrDefaultAsync(u => u.VerificationToken == request.token);
            var loginCourieruser = await _context.Couriers.FirstOrDefaultAsync(u => u.VerificationToken == request.token);
            if (loginuser != null)
            {
                if (loginuser.EmailVerified == true)
                {
                    throw new Exception("Already verified");
                }
                loginuser.EmailVerified = true;
                loginuser.VerifiedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            else if (loginFarmeruser != null)
            {
                if (loginFarmeruser.EmailVerified == true)
                {
                    throw new Exception("Already verified");
                }
                loginFarmeruser.EmailVerified = true;
                loginFarmeruser.VerifiedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            else if (loginCourieruser != null)
            {
                if (loginCourieruser.EmailVerified == true)
                {
                    throw new Exception("Already verified");
                }
                loginCourieruser.EmailVerified = true;
                loginCourieruser.VerifiedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Invalid Token");
            }
            return ("Email Verified");
        }

        // Sending forgot password changing token through email -------------------------------------
        public async Task<string> ForgotPassword(ForgotPasswordDto request)
        {
            var loginuser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginFarmeruser = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginCourieruser = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (loginuser != null)
            {
                loginuser.PasswordResetToken = CreateCustomToken();
                loginuser.ResetTokenExpireAt = DateTime.Now.AddMinutes(10);
                await _context.SaveChangesAsync();
                _emailService.passwordResetEmail(loginuser.Email, loginuser.PasswordResetToken);
            }
            else if (loginFarmeruser != null)
            {
                loginFarmeruser.PasswordResetToken = CreateCustomToken();
                loginFarmeruser.ResetTokenExpireAt = DateTime.Now.AddMinutes(10);
                await _context.SaveChangesAsync();
                _emailService.passwordResetEmail(loginFarmeruser.Email, loginFarmeruser.PasswordResetToken);
            }
            else if (loginCourieruser != null)
            {
                loginCourieruser.PasswordResetToken = CreateCustomToken();
                loginCourieruser.ResetTokenExpireAt = DateTime.Now.AddMinutes(10);
                await _context.SaveChangesAsync();
                _emailService.passwordResetEmail(loginCourieruser.Email, loginCourieruser.PasswordResetToken);
            }
            else
            {
                throw new Exception("Invalid Email!");
            }
            return ("Reset within 10 minutes");
        }

        // Changing the forgot password ---------------------------------------------
        public async Task<string> ResetPassword(ResetPasswordDto request)
        {
            var loginuser = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            var loginFarmeruser = await _context.Farmers.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            var loginCourieruser = await _context.Couriers.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (loginuser != null && loginuser.ResetTokenExpireAt > DateTime.Now)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                loginuser.PasswordResetToken = string.Empty;
                loginuser.ResetTokenExpireAt = DateTime.MinValue.Date;
                loginuser.PasswordHash = passwordHash;
                await _context.SaveChangesAsync();
            }
            else if (loginFarmeruser != null && loginFarmeruser.ResetTokenExpireAt > DateTime.Now)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                loginFarmeruser.PasswordResetToken = string.Empty;
                loginFarmeruser.ResetTokenExpireAt = DateTime.MinValue.Date;
                loginFarmeruser.PasswordHash = passwordHash;
                await _context.SaveChangesAsync();
            }
            else if (loginCourieruser != null && loginCourieruser.ResetTokenExpireAt > DateTime.Now)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                loginCourieruser.PasswordResetToken = string.Empty;
                loginCourieruser.ResetTokenExpireAt = DateTime.MinValue.Date;
                loginCourieruser.PasswordHash = passwordHash;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Invalid Token!");
            }
            return ("Password Successfully Reset");
        }

        // Generating JWT token ---------------------------------------------------------
        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTKey:SecretKey").Value!));
            //var TokenExpireTime = Convert.ToInt64(_configuration.GetSection("JWTKey:TokenExpiryTimeInHour").Value!);
            var TokenExpireTime = Convert.ToInt64(_configuration.GetSection("JWTKey:TokenExpiryTimeInMinutes").Value!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Expires = DateTime.UtcNow.AddHours(TokenExpireTime),
                Expires = DateTime.UtcNow.AddMinutes(TokenExpireTime),
                SigningCredentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Function for generating GUID (Globally Unique IDentifier) -------------------------------------
        public string CreateCustomToken()
        {
            return Guid.NewGuid().ToString();
        }

        // Service for getting user details for display ------------------------------------------
        public async Task<GetDetailsModel> GetUserDetails(string Email)
        {
            var loginuser = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            var loginFarmeruser = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == Email);
            var loginCourieruser = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == Email);
            GetDetailsModel _getUserDetails = new();
            if(loginuser != null)
            {
                _getUserDetails.FName = loginuser.FirstName;
                _getUserDetails.LName = loginuser.LastName;
                _getUserDetails.UserName = loginuser.Username;
                _getUserDetails.AddL1 = loginuser.AddL1;
                _getUserDetails.AddL2 = loginuser.AddL2;
                _getUserDetails.AddL3 = loginuser.AddL3;
                _getUserDetails.PhoneNumber = loginuser.PhoneNumber;
                _getUserDetails.Profilepic = loginuser.ProfileImg;
            }
            else if(loginFarmeruser != null)
            {
                _getUserDetails.FName = loginFarmeruser.FirstName;
                _getUserDetails.LName = loginFarmeruser.LastName;
                _getUserDetails.UserName = loginFarmeruser.Username;
                _getUserDetails.AddL1 = loginFarmeruser.AddL1;
                _getUserDetails.AddL2 = loginFarmeruser.AddL2;
                _getUserDetails.AddL3 = loginFarmeruser.AddL3;
                _getUserDetails.PhoneNumber = loginFarmeruser.PhoneNumber;
                _getUserDetails.Profilepic = loginFarmeruser.ProfileImg;
            }
            else if(loginCourieruser != null)
            {
                _getUserDetails.FName = loginCourieruser.FirstName;
                _getUserDetails.LName = loginCourieruser.LastName;
                _getUserDetails.UserName = loginCourieruser.Username;
                _getUserDetails.AddL1 = loginCourieruser.AddL1;
                _getUserDetails.AddL2 = loginCourieruser.AddL2;
                _getUserDetails.AddL3 = loginCourieruser.AddL3;
                _getUserDetails.PhoneNumber = loginCourieruser.PhoneNumber;
                _getUserDetails.Profilepic = loginCourieruser.ProfileImg;
            }
            else
            {
                throw new Exception("Invalid Email");
            }
            return _getUserDetails;
        }
        
        // Changing user details service -------------------------------------------
        public async Task<string> ChangeUserDetails(ChangeDetailsDto request)
        {
            var loginuser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginFarmeruser = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginCourieruser = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if(loginuser != null)
            {
                loginuser.FirstName = request.FName;
                loginuser.LastName = request.LName;
                loginuser.Username = request.UserName;
                loginuser.AddL1 = request.AddL1;
                loginuser.AddL2 = request.AddL2;
                loginuser.AddL3 = request.AddL3;
                loginuser.PhoneNumber = request.PhoneNumber;
                await _context.SaveChangesAsync();
            }
            else if(loginFarmeruser != null)
            {
                loginFarmeruser.FirstName = request.FName;
                loginFarmeruser.LastName = request.LName;
                loginFarmeruser.Username = request.UserName;
                loginFarmeruser.AddL1 = request.AddL1;
                loginFarmeruser.AddL2 = request.AddL2;
                loginFarmeruser.AddL3 = request.AddL3;
                loginFarmeruser.PhoneNumber = request.PhoneNumber;
                await _context.SaveChangesAsync();
            }
            else if(loginCourieruser != null)
            {
                loginCourieruser.FirstName = request.FName;
                loginCourieruser.LastName = request.LName;
                loginCourieruser.Username = request.UserName;
                loginCourieruser.AddL1 = request.AddL1;
                loginCourieruser.AddL2 = request.AddL2;
                loginCourieruser.AddL3 = request.AddL3;
                loginCourieruser.PhoneNumber= request.PhoneNumber;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Invalid Email");
            }
            return ("Details changed");
        }

        // Service for changing profile image -----------------------------------------------
        public async Task<string> ChangeProfileImg(ChangeProfileImgDto request)
        {
            var loginuser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginFarmeruser = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginCourieruser = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (loginuser != null)
            {
                var existingProfileImg = loginuser.ProfileImg;
                await _fileServices.Delete(existingProfileImg, AzureContainerProfileImg);
                loginuser.ProfileImg = request.ProfileImg;
                await _context.SaveChangesAsync();
            }
            else if (loginFarmeruser != null)
            {
                var existingProfileImg = loginFarmeruser.ProfileImg;
                await _fileServices.Delete(existingProfileImg, AzureContainerProfileImg);
                loginFarmeruser.ProfileImg = request.ProfileImg;
                await _context.SaveChangesAsync();
            }
            else if (loginCourieruser != null)
            {
                var existingProfileImg = loginCourieruser.ProfileImg;
                await _fileServices.Delete(existingProfileImg, AzureContainerProfileImg);
                loginCourieruser.ProfileImg = request.ProfileImg;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Invalid email");
            }
            return ("Profile Img changed");
        }

        // Service for change user password --------------------------------------------------------
        public async Task<string> ChangePwd(ChangePwdDto request)
        {
            var loginuser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginFarmeruser = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == request.Email);
            var loginCourieruser = await _context.Couriers.FirstOrDefaultAsync(u => u.Email == request.Email);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            if (loginuser != null)
            {
                loginuser.PasswordHash = passwordHash;
                await _context.SaveChangesAsync();
            }
            else if (loginFarmeruser != null)
            {
                loginFarmeruser.PasswordHash = passwordHash;
                await _context.SaveChangesAsync();
            }
            else if (loginCourieruser != null)
            {
                loginCourieruser.PasswordHash= passwordHash;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Invalid email");
            }
            return ("Password changed");
        }
    }
}
