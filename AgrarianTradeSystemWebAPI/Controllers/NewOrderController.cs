using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Services.NewOrderServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AgrarianTradeSystemWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NewOrderController : ControllerBase
	{
		private readonly INewOrderServices _orderService;

        public NewOrderController(INewOrderServices orderService)
        {
            _orderService = orderService;
        }
        
		//create new orders
		[HttpPost]
		public async Task<IActionResult> CreateOrder([FromBody] OrderCreationDto orderCreateDto)
		{
			try
			{
				var createdOrder = await _orderService.CreateOrderAsync(orderCreateDto);
				return Ok("order create successfully");
			}
			catch (Exception ex)
			{
				var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
				return BadRequest($"Failed to create order: {errorMessage}");
			}
		}
		[HttpPost("CreateOrders")]
		public async Task<IActionResult> CreateOrders([FromBody] List<OrderCreationDto> orderCreateDtos)
		{
			if (orderCreateDtos == null || !orderCreateDtos.Any())
			{
				return BadRequest("No orders provided.");
			}

			try
			{
				foreach (var orderCreateDto in orderCreateDtos)
				{
					await _orderService.CreateOrderAsync(orderCreateDto);
				}

				return Ok("Orders created successfully");
			}
			catch (Exception ex)
			{
				var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
				return BadRequest($"Failed to create orders: {errorMessage}");
			}
		}

		//get all couriers
		[HttpGet]
		[Route("getcouriers")]
		public async Task<IActionResult> GetCouriers()
		{
			try
			{
				var couriers = await _orderService.GetCourierListAsync();
				return Ok(couriers);
			}
			catch (Exception ex)
			{
				return BadRequest($"Failed to retrieve courier list: {ex.Message}");
			}
		}

		[HttpPut("update-courier/{orderId}")]
		public async Task<IActionResult> UpdateCourierId(int orderId, string courierID)
		{
			try
			{
				await _orderService.UpdateCourierIdAsync(orderId, courierID);
				return Ok("Courier ID updated successfully");
			}
			catch (Exception ex)
			{
				return BadRequest($"Failed to update courier ID: {ex.Message}");
			}
		}

	}
}
