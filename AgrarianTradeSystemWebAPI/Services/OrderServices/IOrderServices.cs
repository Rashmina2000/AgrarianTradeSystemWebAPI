using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgrarianTradeSystemWebAPI.Services.OrderServices
{
    public interface IOrderServices
    {
        Task<List<Orders>> GetCourierOrders(string userId);
        Task<List<Orders>> GetBuyerOrders(string userId);
        Task<List<Orders>> GetFarmerOrders(string farmerId);
        Task UpdateOrderStatus(int orderId, string newStatus);
        Task<List<CourierOrderDto>> GetCourierOrderDetails(int orderId);
        Task<List<FarmerOrderDto>> GetFarmerOrderDetails(int orderId);
        Task<List<BuyerOrderDto>> GetBuyerOrderDetails(int orderId);
    };
}
