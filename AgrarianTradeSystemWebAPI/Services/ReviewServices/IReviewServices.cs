using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgrarianTradeSystemWebAPI.Services.ReviewServices
{
    public interface IReviewServices
    {
      
		Task<Review?> GetSingleReview(int id);

        Task<List<Review>> GetAllReview();

		Task<List<Review>> AddReview(Review review);

        Task<List<Review>?> UpdateReview(int id, Review request);

        Task<List<Review>?> DeleteReview(int id);
   
        Task<Review> AddReviewReply(int id, string reply);
        Task<ReviewDetailsDto> GetReviewDetailsByOrderId(int orderId);
        Task<List<ReviewHistoryDto>> GetAllReviewHistoryByFarmer(string farmerId);
        Task<List<ProductReviewDto>> GetReviewsByProductID(int productId);
        Task<List<ReviewHistoryDto>> GetAllReviewHistory(string buyerId);
        Task<List<ReviewOrdersDto>> GetOrdersToReview(string buyerId);

    }

}
