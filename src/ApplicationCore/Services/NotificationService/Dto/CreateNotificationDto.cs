using Microsoft.Nnn.ApplicationCore.Entities.Notifications;

namespace Microsoft.Nnn.ApplicationCore.Services.NotificationService.Dto
{
    public class CreateNotificationDto
    {
        public string Content { get; set; }
        public NotifyContentType Type { get; set; }
        public long TargetId { get; set; }
        public long OwnerUserId { get; set; }
    }
}