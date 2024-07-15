using AgrarianTradeSystemWebAPI.Data;
using AgrarianTradeSystemWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgrarianTradeSystemWebAPI.Services.EmailService
{
    public class NotificationServices : INotificationServices
    {
        private readonly DataContext _context;
        public NotificationServices(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<List<Notification>> GetNotificationsToEmail(string? email)
        {
            return await _context.Notifications
                                 .Where(d => d.To == email)
                                 .ToListAsync();
        }

        public async Task<Notification> CreateNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return notification;
        }

        public async Task<bool> DeleteNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return false;
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
