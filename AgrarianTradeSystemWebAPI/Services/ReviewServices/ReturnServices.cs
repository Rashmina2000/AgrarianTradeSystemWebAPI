using AgrarianTradeSystemWebAPI.Data;
using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgrarianTradeSystemWebAPI.Services.ReviewServices
{
	public class ReturnServices : IReturnServices
	{
		private readonly DataContext _context;

        public ReturnServices(DataContext context)
        {
            _context = context;
        }

        public async Task<Returns> AddReturn(Returns returnData)
        {
            _context.Returns.Add(returnData);
            await _context.SaveChangesAsync();
            return returnData;
        }

        // Method to get all returns
        public async Task<List<Returns>> GetAllReturns()
        {
            return await _context.Returns.ToListAsync();
        }

        //get orders to return
        public async Task<List<ReturnOrderDto>> GetOrdersToReturn(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                throw new ArgumentException("buyerId cannot be null or empty", nameof(buyerId));
            }

            var returnOrders = await _context.Orders
                .Where(o => o.OrderStatus == "return" && o.BuyerID == buyerId)
                .Select(o => new ReturnOrderDto
                {
                    OrderID = o.OrderID,
                    ProductID = o.ProductID,
                    TotalPrice = o.TotalPrice,
                    TotalQuantity = o.TotalQuantity,
                    ProductName = o.Product != null ? o.Product.ProductTitle : null,
                    ProductDescription = o.Product != null ? o.Product.ProductDescription : null,
                    ProductType = o.Product.ProductType,
                    SellerName = o.Product.Farmer.FirstName,
                    ProductImageUrl = o.Product != null ? o.Product.ProductImageUrl : null
                })
                .ToListAsync();

            return returnOrders;
        }

        public async Task<ReturnDetailsDto> GetDetailsByOrderId(int orderId)
        {
            var returnDto = await _context.Orders
                .Where(o => o.OrderID == orderId)
                .Select(o => new ReturnDetailsDto
                {
                    OrderID = o.OrderID,
                    ProductTitle = o.Product.ProductTitle,
                    ProductDescription = o.Product.ProductDescription,
                    ProductType = o.Product.ProductType,
                    SellerName = o.Product.Farmer.FirstName,
                    ProductImageUrl = o.Product.ProductImageUrl,
                    TotalPrice = o.TotalPrice,
                    TotalQuantity = o.TotalQuantity,
                    BuyerFName = o.Buyer.FirstName,
                    BuyerLName = o.Buyer.LastName,
                    BuyerPNumber = o.Buyer.PhoneNumber,
                    BuyerAddL1 = o.Buyer.AddL1,
                    BuyerAddL2 = o.Buyer.AddL2,
                    BuyerAddL3 = o.Buyer.AddL3,
                    OrderedDate = o.OrderedDate
                })
                .FirstOrDefaultAsync();

            if (returnDto != null)
            {
                // Find the return details based on OrderID
                var returnDetails = await _context.Returns
                    .Where(r => r.OrderID == orderId)
                    .Select(r => new ReturnDto
                    {
                        ReturnId = r.ReturnID,
                        OrderID = r.OrderID,
                        Reason = r.Reason,
                        ReturnQuantity = r.ReturnQuantity,
                        ReturnPrice = r.ReturnPrice,
                        ReturnImageUrl = r.ReturnImageUrl,
                        ReturnDate = r.ReturnDate
                    })
                    .FirstOrDefaultAsync();

                // Merge the return details into the existing returnDto
                if (returnDetails != null)
                {
                    returnDto.ReturnId = returnDetails.ReturnId;
                    returnDto.Reason = returnDetails.Reason;
                    returnDto.ReturnImageUrl = returnDetails.ReturnImageUrl;
                    returnDto.ReturnDate = returnDetails.ReturnDate;
                    returnDto.ReturnQuantity = returnDetails.ReturnQuantity;
                    returnDto.ReturnPrice = returnDetails.ReturnPrice;
                   
                }
            }

            return returnDto;
        }


        public async Task<List<ReturnDto>> GetAllReturnsByFarmer(string farmerId)
        {
            if (string.IsNullOrEmpty(farmerId))
            {
                throw new ArgumentException("farmerId cannot be null or empty", nameof(farmerId));
            }

            var returnOrders = await _context.Returns
                .Where(r => r.Order.Product.FarmerID == farmerId)
                .Select(r => new ReturnDto
                {
                    ReturnId = r.ReturnID,
                    OrderID = r.OrderID,
                    OrderedDate=r.Order.OrderedDate,
                    TotalQuantity = r.Order.TotalQuantity,
                    TotalPrice = r.Order.TotalPrice,
                    ReturnQuantity = r.ReturnQuantity,
                    ReturnPrice = r.ReturnPrice,
                    ProductTitle = r.Order.Product != null ? r.Order.Product.ProductTitle : null,
                    ProductDescription = r.Order.Product != null ? r.Order.Product.ProductDescription : null,
                    ProductImageUrl = r.Order.Product != null ? r.Order.Product.ProductImageUrl : null,
                    ProductType= r.Order.Product != null ? r.Order.Product.ProductType : null,
                    Reason = r.Reason,
                    ReturnImageUrl = r.ReturnImageUrl,
                    ReturnDate = r.ReturnDate
                })
                .ToListAsync();

            return returnOrders;
        }


    }
}
