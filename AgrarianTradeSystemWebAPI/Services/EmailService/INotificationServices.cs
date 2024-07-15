using AgrarianTradeSystemWebAPI.Models;

namespace AgrarianTradeSystemWebAPI.Services.EmailService
{
    public interface INotificationServices
    {
        Task<List<Notification>> GetNotifications();
        Task<Notification> CreateNotification(Notification notification);

        Task<List<Notification>> GetNotificationsToEmail(string? email);

        Task<bool> DeleteNotification(int id);
    }
}
