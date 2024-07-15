using AgrarianTradeSystemWebAPI.Data;
using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgrarianTradeSystemWebAPI.Services.NewOrderServices
{
	public class NewOrderServices :INewOrderServices
	{
		private readonly DataContext _context;

        public NewOrderServices(DataContext context)
        {
            _context = context;
        }

		public async Task<Orders> CreateOrderAsync(OrderCreationDto orderCreateModel)
		{
			// Create a new order
			var order = new Orders
			{
				BuyerID = orderCreateModel.BuyerID,
				ProductID = orderCreateModel.ProductID,
				DeliveryAddressLine1 = orderCreateModel.DeliveryAddressLine1,
				DeliveryAddressLine2 = orderCreateModel.DeliveryAddressLine2,
				DeliveryAddressLine3 = orderCreateModel.DeliveryAddressLine3,
				OrderStatus = orderCreateModel.OrderStatus,
				DeliveryFee = orderCreateModel.DeliveryFee,
				OrderedDate = orderCreateModel.OrderedDate,
				TotalPrice = orderCreateModel.TotalPrice,
				CourierID = orderCreateModel.CourierID,
				PickupDate = orderCreateModel.PickupDate,
				DeliveryDate = orderCreateModel.DeliveryDate,
				TotalQuantity = orderCreateModel.TotalQuantity
			};

			// Add the order to the context
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			// Fetch the related product
			var product = await _context.Products.FindAsync(orderCreateModel.ProductID);
			if (product != null)
			{
				// Increment the OrdersCount for the product
				product.OrdersCount += 1;

				// Save the changes to the product
				await _context.SaveChangesAsync();
			}
			else
			{
				// Handle the case where the product is not found
				throw new Exception("Product not found");
			}

			return order;
		}



		//get couriers list
		public async Task<List<CourierListDto>> GetCourierListAsync()
		{
			
			var couriers = await _context.Couriers.ToListAsync();

			var courierListDto = new List<CourierListDto>();
			foreach (var courier in couriers)
			{
				courierListDto.Add(new CourierListDto
				{
					CourierID = courier.Email,
					CourierFName = courier.FirstName,
					CourierLName = courier.LastName,
					CourierImageUrl = courier.ProfileImg,
					AddressLine1 = courier.AddL1,
					AddressLine2 = courier.AddL2,
					AddressLine3 = courier.AddL3
				});
			}

			return courierListDto;
		}


		public async Task UpdateCourierIdAsync(int orderId, string newCourierId)
		{
			var order = await _context.Orders.FindAsync(orderId);
			if (order == null)
			{
				throw new Exception("Order not found");
			}
			var courier = await _context.Couriers.FindAsync(newCourierId);
			if (courier == null)
			{
				throw new Exception("Courier not found");
			}

			order.CourierID = newCourierId;
			await _context.SaveChangesAsync();
		}

	}

}
