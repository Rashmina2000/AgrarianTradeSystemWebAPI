using AgrarianTradeSystemWebAPI.Data;
using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using AgrarianTradeSystemWebAPI.Models.UserModels;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;

namespace AgrarianTradeSystemWebAPI.Services.OrderServices
{
    public class OrderServices : IOrderServices
    {
        private readonly DataContext _context;

        public OrderServices(DataContext context)
        {
            _context = context;
        }


        //get courier's order
        public async Task<List<Orders>> GetCourierOrders(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Product )
                .ThenInclude(p => p.Farmer)
                .Include(o => o.Buyer)
                .Include(o => o.Courier)
                .Where(o => o.CourierID == userId)
                .ToListAsync();
            return orders;
        }

        // Get buyer's orders
        public async Task<List<Orders>> GetBuyerOrders(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .ThenInclude(p => p.Farmer)
                .Include(o => o.Courier)
                .Where(o => o.BuyerID == userId)
                .ToListAsync();
            return orders;
        }

        // Get farmer's orders
        public async Task<List<Orders>> GetFarmerOrders(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .Include(p => p.Buyer)
                .Include(o => o.Courier)
                .Where(o => o.Product.FarmerID == userId)
                .ToListAsync();
            return orders;
        }

        //update order status
        public async Task UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == orderId);
            if (order != null)
            {
             
                    order.OrderStatus = newStatus; // Update the status to "Picked up"
                    await _context.SaveChangesAsync(); // Save changes to the database
             
            }
            else
            {
                throw new ArgumentException("Order not found.");
            }
        }

        //get courier's order details
        public async Task<List<CourierOrderDto>> GetCourierOrderDetails(int orderId)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Product)
                    .ThenInclude(p => p.Farmer)
                    .Include(o => o.Buyer)
                    .Include(o => o.Courier)
                    .Where(o => o.OrderID == orderId)
                    .ToListAsync();

                var orderDtos = orders.Select(order => new CourierOrderDto
                {
                    OrderID = order.OrderID,
                    BuyerID = order.BuyerID,
                    CourierID = order.CourierID,
                    FarmerID=order.Product.FarmerID,
                    ProductTitle = order.Product?.ProductTitle,
                    ProductImageUrl = order.Product?.ProductImageUrl,
                    OrderStatus = order.OrderStatus,
                    TotalQuantity = order.TotalQuantity,
					DeliveryFee =order.DeliveryFee,
					DeliveryDate = order.DeliveryDate,
					PickupDate = order.PickupDate,
					OrderedDate = order.OrderedDate,    
					CustomerFName = order.Buyer?.FirstName,
                    CustomerLName = order.Buyer?.LastName,
                    CustomerAddL1 = order.Buyer?.AddL1,
                    CustomerAddL2 = order.Buyer?.AddL2,
                    CustomerAddL3 = order.Buyer?.AddL3,
                    CustomerPhoneNumber = order.Buyer.PhoneNumber,
                    FarmerFName = order.Product.Farmer?.FirstName,
                    FarmerLName = order.Product.Farmer?.LastName,
                    FarmerAddL1 = order.Product.Farmer?.AddL1,
                    FarmerAddL2 = order.Product.Farmer?.AddL2,
                    FarmerAddL3 = order.Product.Farmer?.AddL3,
                    FarmerPhoneNumber = order.Product.Farmer?.PhoneNumber
                    
                }).ToList();

                return orderDtos;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Failed to get courier order details.", ex);
            }
        }

        //get farmer's order details
        public async Task<List<FarmerOrderDto>> GetFarmerOrderDetails(int orderId)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Product)
                    .Include(p => p.Buyer)
                    .Include(o => o.Courier)
                    .Where(o => o.OrderID == orderId)
                    .ToListAsync();

                var orderDtos = orders.Select(order => new FarmerOrderDto
                {
                    OrderID = order.OrderID,
                    ProductTitle = order.Product.ProductTitle,
                    ProductImageUrl = order.Product.ProductImageUrl,
                    OrderStatus = order.OrderStatus,
                    TotalQuantity = order.TotalQuantity,
                    CourierFName = order.Courier?.FirstName,
                    CourierLName = order.Courier?.LastName,
                    CourierAddL1 = order.Courier?.AddL1,
                    CourierAddL2 = order.Courier?.AddL2,
                    CourierAddL3 = order.Courier?.AddL3,
                    CourierPhoneNumber = order.Courier?.PhoneNumber,
                    CustomerFName = order.Buyer?.FirstName,
                    CustomerLName = order.Buyer?.LastName,
                    CustomerAddL1 = order.Buyer?.AddL1,
                    CustomerAddL2 = order.Buyer?.AddL2,
                    CustomerAddL3 = order.Buyer?.AddL3,
                    CustomerPhoneNumber = order.Buyer?.PhoneNumber
                }).ToList();

                return orderDtos;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Failed to get farmer order details.", ex);
            }
        }

        //get buyer's order details
        public async Task<List<BuyerOrderDto>> GetBuyerOrderDetails(int orderId)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Product)
                    .ThenInclude(p => p.Farmer)
                    .Include(o => o.Courier)
                    .Where(o => o.OrderID == orderId)
                    .ToListAsync();

                var orderDtos = orders.Select(order => new BuyerOrderDto
                {
                    OrderID = order.OrderID,
                    ProductTitle = order.Product?.ProductTitle,
                    ProductImageUrl = order.Product.ProductImageUrl,
                    ProductDescription = order.Product.ProductDescription,
                    ProductType = order.Product.ProductType,
                    TotalPrice = order.TotalPrice,
                    OrderStatus = order.OrderStatus,
                    TotalQuantity = order.TotalQuantity,
                    FarmerFName = order.Product.Farmer?.FirstName,
                    FarmerLName = order.Product.Farmer?.LastName,
                    FarmerAddL1 = order.Product.Farmer?.AddL1,
                    FarmerAddL2 = order.Product.Farmer?.AddL2,
                    FarmerAddL3 = order.Product.Farmer?.AddL3,
                    FarmerPhoneNumber = order.Product.Farmer?.PhoneNumber,
                    CourierFName = order.Courier?.FirstName,
                    CourierLName = order.Courier?.LastName,
                    CourierAddL1 = order.Courier?.AddL1,
                    CourierAddL2 = order.Courier?.AddL2,
                    CourierAddL3 = order.Courier?.AddL3,
                    CourierPhoneNumber = order.Courier?.PhoneNumber
                }).ToList();

                return orderDtos;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Failed to get buyer order details.", ex);
            }
        }
    }
}

