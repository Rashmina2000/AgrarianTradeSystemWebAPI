using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using AgrarianTradeSystemWebAPI.Services.ProductServices;
using AgrarianTradeSystemWebAPI.Services.ReviewServices;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgrarianTradeSystemWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{

		private readonly IReviewServices _reviewServices;
		private readonly IFileServices _fileServices;
		private const string AzureContainerName = "reviews";

		public ReviewController(IReviewServices reviewServices, IFileServices fileServices)
		{
			_reviewServices = reviewServices;
			_fileServices = fileServices;

		}

        [HttpGet("order-details/{orderId}")]
        public async Task<ActionResult<ReviewDetailsDto>> GetReviewDetailsByOrderId(int orderId)
        {
            var reviewDetails = await _reviewServices.GetReviewDetailsByOrderId(orderId);
            if (reviewDetails == null)
            {
                return NotFound();
            }
            return Ok(reviewDetails);
        }
        [HttpPost("add-review")]
		public async Task<ActionResult<List<Review>>> AddReview([FromForm] AddReviewDto reviewDto, IFormFile file)
		{
			try
			{
				if (file == null || file.Length == 0)
				{
					return BadRequest("No file uploaded.");
				}

				var fileUrl = await _fileServices.Upload(file, AzureContainerName);

				var review = new Review
				{
					OrderID = reviewDto.OrderID,
					Comment = reviewDto.Comment,
					SellerRating = reviewDto.SellerRating,
					DeliverRating = reviewDto.DeliverRating,
					ProductRating = reviewDto.ProductRating,
					ReviewImageUrl = fileUrl
				};

				var result = await _reviewServices.AddReview(review);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An unexpected error occurred while processing the request." + ex.Message);
			}

		}

		[HttpPut("edit-review/{id}")]
		public async Task<ActionResult<List<Review>>> UpdateReview(int id, Review request)
		{

			var result = await _reviewServices.UpdateReview(id, request);
			if (result is null)
				return NotFound("review is not found");

			return Ok(result);

		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<List<Review>>> DeleteReview(int id)
		{

			var result = await _reviewServices.DeleteReview(id);
			if (result is null)
				return NotFound("hero is not found");
			return Ok(result);
		}


        [HttpGet("to-review/buyer")]
        public async Task<IActionResult> GetOrdersWithStatusReview([FromQuery] string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                return BadRequest("BuyerID is required.");
            }

            var orders = await _reviewServices.GetOrdersToReview(buyerId);
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found for the given BuyerID.");
            }

            return Ok(orders);
        }


        [HttpPut("add-reply/{id}")]
		public async Task<IActionResult> UpdateReviewReply(int id,string reply)
		{
			var updatedReview = await _reviewServices.AddReviewReply(id, reply);

			if (updatedReview == null)
			{
				return NotFound();
			}

			return Ok(updatedReview);
		}

		[HttpGet("product-reviews/{productId}")]
		public async Task<IActionResult> GetReviewsByProductID(int productId)
		{
			var reviews = await _reviewServices.GetReviewsByProductID(productId);
			return Ok(reviews);
		}


        [HttpGet("review-history")]
        public async Task<IActionResult> GetReviewHistory(string buyerId)
        {
            var reviews = await _reviewServices.GetAllReviewHistory(buyerId);
            return Ok(reviews);
        }

        [HttpGet("reviews/farmer")]
        public async Task<IActionResult> GetReviewsByFarmer([FromQuery] string farmerId)
        {
            if (string.IsNullOrEmpty(farmerId))
            {
                return BadRequest("FarmerID cannot be null or empty.");
            }

            try
            {
                var reviewHistory = await _reviewServices.GetAllReviewHistoryByFarmer(farmerId);
                return Ok(reviewHistory);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework)
                return StatusCode(500, "An error occurred while retrieving review history.");
            }
        }

    }


}

