using AgrarianTradeSystemWebAPI.Data;
using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgrarianTradeSystemWebAPI.Services.ReviewServices
{
	public class ReviewServices : IReviewServices
	{
		private readonly DataContext _context;
		public ReviewServices(DataContext context)
		{
			_context = context;
		}
		public DataContext Context { get; }


		//get all data
		public async Task<List<Review>> GetAllReview()
		{

			var reviews = await _context.Reviews.ToListAsync(); //retrieve data from db
			return reviews;
		}

		//get single data by id
		public async Task<Review?> GetSingleReview(int id)
		{
			var review = await _context.Reviews.FindAsync(id);  //find data from db
			if (review == null)
				return null;
			return review;
		}
        public async Task<ReviewDetailsDto> GetReviewDetailsByOrderId(int orderId)
        {
            var reviewDto = await _context.Orders
                .Where(o => o.OrderID == orderId)
                .Select(o => new ReviewDetailsDto
                {
                    OrderID = o.OrderID,
                    productId=o.ProductID,
                    ProductTitle = o.Product.ProductTitle,
                    ProductDescription = o.Product.ProductDescription,
                    ProductType = o.Product.ProductType,
                    ProductImageUrl = o.Product.ProductImageUrl,
                    TotalPrice = o.TotalPrice,
                    TotalQuantity = o.TotalQuantity,
                    BuyerFName = o.Buyer.FirstName,
                    BuyerLName = o.Buyer.LastName,
                    BuyerImageUrl=o.Buyer.ProfileImg,
                    OrderedDate = o.OrderedDate
                })
                .FirstOrDefaultAsync();

            if (reviewDto != null)
            {
                // Now, find the review details based on OrderID
                var reviewDetails = await _context.Reviews
                    .Where(r => r.OrderID == orderId)
                    .Select(r => new
                    {
                        r.ReviewId,
                        r.Comment,
                        r.ReviewImageUrl,
                        r.ReviewDate,
                        r.SellerRating,
                        r.DeliverRating,
                        r.ProductRating,
                        r.Reply
                    })
                    .FirstOrDefaultAsync();

                // Merge the review details into the existing reviewDto
                if (reviewDetails != null)
                {
                    reviewDto.ReviewId = reviewDetails.ReviewId;
                    reviewDto.Comment = reviewDetails.Comment;
                    reviewDto.ReviewImageUrl = reviewDetails.ReviewImageUrl;
                    reviewDto.ReviewDate = reviewDetails.ReviewDate;
                    reviewDto.SellerRating = reviewDetails.SellerRating;
                    reviewDto.DeliverRating = reviewDetails.DeliverRating;
                    reviewDto.ProductRating = reviewDetails.ProductRating;
                    reviewDto.Reply = reviewDetails.Reply;
                }
            }

            return reviewDto;
        }

        //add
        public async Task<List<Review>> AddReview(Review review)
		{
			_context.Reviews.Add(review);
			await _context.SaveChangesAsync();
			return await _context.Reviews.ToListAsync();
		}


		//update review
		public async Task<List<Review>?> UpdateReview(int id, Review request)
		{
			var review = await _context.Reviews.FindAsync(id); //find data from db
			if (review == null)
				return null;

			review.Comment = request.Comment;
			review.SellerRating = request.SellerRating;
			review.ReviewImageUrl = request.ReviewImageUrl;
			review.ProductRating = request.ProductRating;
			review.DeliverRating = request.DeliverRating;


			await _context.SaveChangesAsync();


			return await _context.Reviews.ToListAsync();
		}

		//delete review
		public async Task<List<Review>?> DeleteReview(int id)
		{
			var review = await _context.Reviews.FindAsync(id); //find data from db
			if (review == null)
				return null;

			_context.Reviews.Remove(review);

			await _context.SaveChangesAsync();
			return await _context.Reviews.ToListAsync();
		}
        //get orders to return
        public async Task<List<ReviewOrdersDto>> GetOrdersToReview(string buyerId)
        {
            var reviewOrders = await _context.Orders
                .Where(o => o.OrderStatus == "review" && o.BuyerID == buyerId)
                .Select(o => new ReviewOrdersDto
                {
                    OrderID = o.OrderID,
                    TotalQuantity = o.TotalQuantity,
                    OrderedDate = o.OrderedDate,
                    ProductType= o.Product.ProductType,
                    ProductID = o.ProductID,
                    ProductName = o.Product.ProductTitle,
                    ProductDescription = o.Product.ProductDescription,
                    ProductImageUrl = o.Product.ProductImageUrl
                })
                .ToListAsync();

            return reviewOrders;
        }


        public async Task<Review> AddReviewReply(int id, string reply)
		{
			var review = await _context.Reviews.FindAsync(id);

			if (review == null)
			{
				return null; // Return null if review with the given ID is not found
			}

			review.Reply = reply;
			await _context.SaveChangesAsync();

			return review;
		}

        public async Task<List<ProductReviewDto>> GetReviewsByProductID(int productId)
        {
            // Step 1: Retrieve OrderIDs for the given ProductID
            var orderIds = await _context.Orders
                .Where(o => o.ProductID == productId)
                .Select(o => o.OrderID)
                .ToListAsync();

            // Step 2: Fetch Reviews using the OrderIDs
            var reviews = await _context.Reviews
                .Where(r => orderIds.Contains(r.OrderID))
                .Include(r => r.Order) // Include Orders for mapping
                .ThenInclude(o => o.Buyer) // Include Buyer for mapping
                .ToListAsync();

            // Step 3: Map Reviews to ProductReviewDto
            var productReviewDtos = reviews.Select(r => new ProductReviewDto
            {
                ReviewId = r.ReviewId,
                OrderID = r.OrderID,
                BuyerFirstName = r.Order?.Buyer?.FirstName ?? "Unknown",
                BuyerLastName = r.Order?.Buyer?.LastName ?? "Unknown",
                BuyerProfileImageUrl = r.Order?.Buyer?.ProfileImg,
                SellerFirstName=r.Order?.Product?.Farmer?.FirstName,
                SellerLastName=r.Order?.Product?.Farmer?.LastName,
                SellerProfileImageUrl=r.Order?.Product?.Farmer?.ProfileImg,
                Comment = r.Comment,
                ReviewImageUrl = r.ReviewImageUrl,
                ReviewDate = r.ReviewDate,
                SellerRating = r.SellerRating,
                DeliverRating = r.DeliverRating,
                ProductRating = r.ProductRating,
                Reply = r.Reply
            }).ToList();

            // Step 4: Return the list of ProductReviewDto
            return productReviewDtos;
        }

        public async Task<List<ReviewHistoryDto>> GetAllReviewHistory(string buyerId)
        {
            var reviewDetails = await _context.Reviews
                .Include(r => r.Order)
                .ThenInclude(o => o.Product)
                .Where(r => r.Order.BuyerID == buyerId)
                .Select(r => new ReviewHistoryDto
                {
                    ReviewId = r.ReviewId,
                    OrderID = r.OrderID,
                    ProductTitle = r.Order.Product != null ? r.Order.Product.ProductTitle : null,
                    ProductDescription = r.Order.Product != null ? r.Order.Product.ProductDescription : null,
                    ProductImageUrl = r.Order.Product != null ? r.Order.Product.ProductImageUrl : null,
                    OrderedDate = r.Order.OrderedDate,
                    ProductType = r.Order.Product != null ? r.Order.Product.ProductType : null,
                    Comment = r.Comment,
                    ReviewImageUrl = r.ReviewImageUrl,
                    ReviewDate = r.ReviewDate,
                    SellerRating = r.SellerRating,
                    DeliverRating = r.DeliverRating,
                    ProductRating = r.ProductRating
                })
                .ToListAsync();

            return reviewDetails;
        }

        public async Task<List<ReviewHistoryDto>> GetAllReviewHistoryByFarmer(string farmerId)
        {
            if (string.IsNullOrEmpty(farmerId))
            {
                throw new ArgumentException("farmerId cannot be null or empty", nameof(farmerId));
            }

            var reviewDetails = await _context.Reviews
                .Include(r => r.Order)
                    .ThenInclude(o => o.Product)
                .Where(r => r.Order != null && r.Order.Product != null && r.Order.Product.FarmerID == farmerId)
                .Select(r => new ReviewHistoryDto
                {
                    ReviewId = r.ReviewId,
                    OrderID = r.OrderID,
                    TotalQuantity=r.Order.TotalQuantity,
                    ProductTitle = r.Order.Product.ProductTitle,
                    ProductDescription = r.Order.Product.ProductDescription,
                    ProductImageUrl = r.Order.Product.ProductImageUrl,
                    OrderedDate = r.Order.OrderedDate,
                    ProductType = r.Order.Product != null ? r.Order.Product.ProductType : null,
                    Comment = r.Comment,
                    ReviewImageUrl = r.ReviewImageUrl,
                    ReviewDate = r.ReviewDate,
                    SellerRating = r.SellerRating,
                    DeliverRating = r.DeliverRating,
                    ProductRating = r.ProductRating
                })
                .ToListAsync();

            return reviewDetails;
        }


    }

}
