using AgrarianTradeSystemWebAPI.Models.AdminModels;
using AgrarianTradeSystemWebAPI.Models.UserModels;
using AgrarianTradeSystemWebAPI.Services.AdminServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgrarianTradeSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        // Admin login ----------------------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            try
            {
                var result = await _adminServices.Login(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Create admin -----------------------------------------------
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdmin(AdminDto request)
        {
            try
            {
                await _adminServices.CreateAdmin(request);
                return Ok("Admin created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // Request for getting new couriers -----------------------------------
        [HttpGet("NewCouriers")]
        public async Task<ActionResult<List<GetCourierModel>>> GetAllNewCouriers()
        {
            return await _adminServices.GetAllNewCouriers();
        }

        // Request for getting approved couriers --------------------------------
        [HttpGet("ApprovedCouriers")]
        public async Task<ActionResult<List<GetCourierModel>>> GetAllApprovedCouriers()
        {
            return await _adminServices.GetAllApprovedCouriers();
        }

        // Request for getting new farmers -------------------------------------
        [HttpGet("NewFarmers")]
        public async Task<ActionResult<List<GetFarmerModel>>> GetAllNewFarmers()
        {
            return await _adminServices.GetAllNewFarmers();
        }

        // Request for getting approved farmers --------------------------------
        [HttpGet("ApprovedFarmers")]
        public async Task<ActionResult<List<GetFarmerModel>>> GetAllApprovedFarmers()
        {
            return await _adminServices.GetAllApprovedFarmers();
        }
        
        // Request for approve a farmer -----------------------------------------
        [HttpPut("approveFarmer")]
        public async Task<IActionResult> ApproveFarmer(string email)
        {
            try
            {
                var result = await _adminServices.ApproveFarmer(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for approve a courier --------------------------------------
        [HttpPut("approveCourier")]
        public async Task<IActionResult> ApproveCourier(string email)
        {
            try
            {
                var result = await _adminServices.ApproveCourier(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // Request for reject a farmer -------------------------------------------
        [HttpPost("denyFarmer")]
        public async Task<IActionResult> DenyFarmer(UserDenyDto request)
        {
            try
            {
                var result = await _adminServices.DenyFarmer(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Request for reject a courier ----------------------------------------
        [HttpPost("denyCourier")]
        public async Task<IActionResult> DenyCourier(UserDenyDto request)
        {
            try
            {
                var result = await _adminServices.DenyCourier(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
