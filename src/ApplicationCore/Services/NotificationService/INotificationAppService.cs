using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Services.NotificationService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.NotificationService
{
    public interface INotificationAppService
    {
        Task<List<NotificationDto>> GetUserNotifications(long userId);
        Task<long> GetUnReads(long userId);
        Task MarkAsRead(long[] ids);
    }
}