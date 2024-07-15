using AgrarianTradeSystemWebAPI.Models;
using AgrarianTradeSystemWebAPI.Services.EmailService;
using AgrarianTradeSystemWebAPI.Services.NewOrderServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgrarianTradeSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationServices _notificationServices;

        public NotificationController(INotificationServices notificationServices)
        {
            _notificationServices = notificationServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var notifications = await _notificationServices.GetNotifications();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve courier list: {ex.Message}");
            }
        }
        //create new notification
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            try
            {
                var notifi = await _notificationServices.CreateNotification(notification);
                return Ok("notification sent");
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest($"Failed to create order: {errorMessage}");
            }
        }

        [HttpGet]
        [Route("to/{id}")]
        public async Task<IActionResult> GetNotifications(string id)
        {
            try
            {
                var notifications = await _notificationServices.GetNotificationsToEmail(id);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve courier list: {ex.Message}");
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var result = await _notificationServices.DeleteNotification(id);
                if (!result)
                {
                    return NotFound($"Notification with ID {id} not found.");
                }

                return Ok($"Notification with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete notification: {ex.Message}");
            }
        }

    }
}
