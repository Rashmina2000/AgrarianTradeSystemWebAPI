using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using AgrarianTradeSystemWebAPI.Services.ProductServices;
using AgrarianTradeSystemWebAPI.Services.ReviewServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgrarianTradeSystemWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReturnController : ControllerBase
	{
        public readonly IReturnServices _returnServices;
        public readonly IFileServices _fileServices;
        private const string AzureContainerName = "returns";
        public ReturnController(IReturnServices returnServices ,IFileServices fileServices)
        {
            _returnServices = returnServices;
            _fileServices = fileServices;
        }


        [HttpPost("add-return")]
        public async Task<ActionResult<Returns>> AddReturn([FromForm] AddReturnDto returnDto, IFormFile? file)
        {
            try
            {
                string? fileUrl = null;

                if (file != null && file.Length > 0)
                {
                    fileUrl = await _fileServices.Upload(file, AzureContainerName);
                }
                var returns = new Returns
                {
                    OrderID = returnDto.OrderID,
                    Reason = returnDto.Reason,
                    ReturnQuantity=returnDto.ReturnQuantity,
                    ReturnPrice=returnDto.ReturnPrice,
                    ReturnDate = DateTime.Now,
                    ReturnImageUrl = fileUrl
                };
                var result = await _returnServices.AddReturn(returns);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while processing the request. " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Returns>>> GetAllReturns()
        {
            try
            {
                var result = await _returnServices.GetAllReturns();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("return/buyer")]
        public async Task<IActionResult> GetOrdersWithStatusReview([FromQuery] string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                return BadRequest("BuyerID cannot be null or empty.");
            }

            var orders = await _returnServices.GetOrdersToReturn(buyerId);
            return Ok(orders);
        }

        [HttpGet("return-details/{orderId}")]
        public async Task<ActionResult<ReturnDto>> GetDetailsByOrderId(int orderId)
        {
            try
            {
                var returnDto = await _returnServices.GetDetailsByOrderId(orderId);

                if (returnDto == null)
                {
                    return NotFound(); 
                }

                return Ok(returnDto); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while processing the request. " + ex.Message);
            }
        }

        [HttpGet("return/farmer")]
        public async Task<IActionResult> GetReturnByFarmer([FromQuery] string farmerId)
        {
            if (string.IsNullOrEmpty(farmerId))
            {
                return BadRequest("FarmerID cannot be null or empty.");
            }

            var returnOrders = await _returnServices.GetAllReturnsByFarmer(farmerId);
            return Ok(returnOrders);
        }


    }
}
