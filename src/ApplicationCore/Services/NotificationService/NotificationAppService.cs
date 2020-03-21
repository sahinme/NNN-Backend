using System.Linq;
using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Notifications;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.NotificationService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.NotificationService
{
    public class NotificationAppService:INotificationAppService
    {
        private readonly IAsyncRepository<Notification> _notificationRepository;

        public NotificationAppService(IAsyncRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task CreateNotify(CreateNotificationDto input)
        {
            var notify = new Notification
            {
                OwnerId = input.OwnerId,
                SenderId = input.SenderId,
                Title = input.Title,
                ContentId = input.ContentId,
                Content = input.Content,
            };
            await _notificationRepository.AddAsync(notify);
        }

        public async Task<bool> MarkAsRead(long id)
        {
            var notify = await _notificationRepository.GetByIdAsync(id);
            notify.IsRead = true;
            await _notificationRepository.UpdateAsync(notify);
            return true;
        }
    }
}