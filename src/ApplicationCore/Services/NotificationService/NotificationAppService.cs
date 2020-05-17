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

//        public async Task<Notification> Create(CreateNotificationDto input)
//        {
//            switch (input.Type)
//            {
//                case NotifyContentType.PostVote:
//                    
//                    
//            }
//        }
    }
}