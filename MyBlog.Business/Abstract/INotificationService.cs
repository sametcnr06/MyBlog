using MyBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Business.Abstract
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string message, NotificationType type, int? targetId = null);
        Task<List<Notification>> GetUnreadNotificationsAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
        // vs...
    }
}
