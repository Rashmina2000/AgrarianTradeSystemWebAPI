using Microsoft.AspNetCore.Mvc;
using AgrarianTradeSystemWebAPI.Models.UserModels;
using Microsoft.AspNetCore.Cors;
using AgrarianTradeSystemWebAPI.Services.UserServices;
using AgrarianTradeSystemWebAPI.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;

namespace AgrarianTradeSystemWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("ReactJSDomain")]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IFileServices _fileServices;
        private const string AzureContainerProfileImg = "profilepic";
        private const string AzureContainerNICImg = "nicimage";
        private const string AzureContainerVehicleImg = "vehicleimage";
        private const string AzureContainerGNSImg = "gramaniladhari";
        public AuthController(IUserServices userServices, IFileServices fileServices)
        {
            _userServices = userServices;
            _fileServices = fileServices;

        }

        // Request for user registration -----------------------------------
        [HttpPost("UserRegister")]
        public async Task<IActionResult> Register([FromForm] UserDto request, IFormFile profilepic)
        {
            if (profilepic == null || profilepic.Length == 0)
            {
                return BadRequest("No profile pic uploaded.");
            }
            
            try
            {
                var profileUrl = await _fileServices.Upload(profilepic, AzureContainerProfileImg);
                request.ProfileImg = profileUrl;
                await _userServices.UserRegister(request);
                return Ok("User created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for farmer registration -----------------------------------------
        [HttpPost("FarmerRegister")]
        public async Task<IActionResult> FarmerRegister([FromForm] FarmerDto request, IFormFile profilepic, IFormFile nicfront, IFormFile nicback, IFormFile gncertificate)
        {
            if (profilepic == null || profilepic.Length == 0)
            {
                return BadRequest("No profile pic uploaded.");
            }
            if (nicfront == null || nicfront.Length == 0)
            {
                return BadRequest("No NIC front uploaded.");
            }
            if (nicback == null || nicback.Length == 0)
            {
                return BadRequest("No NIC back uploaded.");
            }
            if (gncertificate == null || gncertificate.Length == 0)
            {
                return BadRequest("No GN Certificate uploaded.");
            }

            try
            {
                var profileUrl = await _fileServices.Upload(profilepic, AzureContainerProfileImg);
                var nicFrontUrl = await _fileServices.Upload(nicfront, AzureContainerNICImg);
                var nicBackUrl = await _fileServices.Upload(nicback, AzureContainerNICImg);
                var gsCertificateUrl = await _fileServices.Upload(gncertificate, AzureContainerGNSImg);
                request.ProfileImg = profileUrl;
                request.NICFrontImg = nicFrontUrl;
                request.NICBackImg = nicBackUrl;
                request.GNCImage = gsCertificateUrl;
                await _userServices.FarmerRegister(request);
                return Ok("User created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for courier registration ---------------------------------------
        [HttpPost("CourierRegister")]
        public async Task<IActionResult> CourierRegister([FromForm] CourierDto request, IFormFile profilepic, IFormFile vehicle, IFormFile license)
        {
            if (profilepic == null || profilepic.Length == 0)
            {
                return BadRequest("No profile pic uploaded.");
            }
            if (vehicle == null || vehicle.Length == 0)
            {
                return BadRequest("No vehicle pic uploaded.");
            }
            if (license == null || license.Length == 0)
            {
                return BadRequest("No license uploaded.");
            }

            try
            {
                var profileUrl = await _fileServices.Upload(profilepic, AzureContainerProfileImg);
                var vehicleUrl = await _fileServices.Upload(vehicle, AzureContainerVehicleImg);
                var licenseUrl = await _fileServices.Upload(license, AzureContainerNICImg);
                request.ProfileImg = profileUrl;
                request.VehicleImg = vehicleUrl;
                request.LicenseImg = licenseUrl;
                await _userServices.CourierRegister(request);
                return Ok("User created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for user login ----------------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            try
            {
                var result = await _userServices.Login(request);
                return Ok(result);
            }
            catch (LoginException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for getting verification link ----------------------------------------------
        [HttpPost("getVerifyLink")]
        public async Task<IActionResult> VerifyLink(GetVerifyLinkDto request)
        {
            try
            {
                var result = await _userServices.GetVerifyLink(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for verify the email -----------------------------------------------
        [HttpPost("verify")]
        public async Task<IActionResult> Verify(VerifyDto request)
        {
            try
            {
                var result = await _userServices.Verify(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for getting forget password reset token ----------------------------------
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            try
            {
                var result = await _userServices.ForgotPassword(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for change forget password ---------------------------------------
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            try
            {
                var result = await _userServices.ResetPassword(request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for getting user's details ---------------------------------
        //[Authorize]
        [HttpGet("getUserDetails")]
        public async Task<IActionResult> GetUserDetails(string email)
        {
            try
            {
                var result = await _userServices.GetUserDetails(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for changing the user's details -------------------------------------
        [HttpPost("changeUserDetails")]
        public async Task<IActionResult> ChangeUserDetails(ChangeDetailsDto request)
        {
            try
            {
                var result = await _userServices.ChangeUserDetails(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for change profile image --------------------------------
        [HttpPost("changeProfileImg")]
        public async Task<IActionResult> ChangeProfileImg([FromForm] ChangeProfileImgDto request, IFormFile profilepic)
        {
            if (profilepic == null || profilepic.Length == 0)
            {
                return BadRequest("No profile pic uploaded.");
            }
            try
            {
                var newProfileUrl = await _fileServices.Upload(profilepic, AzureContainerProfileImg);
                request.ProfileImg = newProfileUrl;
                var result = await _userServices.ChangeProfileImg(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // Request for change current password -------------------------------------
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePwdDto request)
        {
            try
            {
                var result = await _userServices.ChangePwd(request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
