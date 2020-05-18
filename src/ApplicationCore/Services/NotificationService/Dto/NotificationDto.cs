using Microsoft.Nnn.ApplicationCore.Entities.Notifications;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;

namespace Microsoft.Nnn.ApplicationCore.Services.NotificationService.Dto
{
    public class NotificationDto
    {
        public long Id { get; set; }
        public NotifyContentType Type { get; set; }
        public string Content { get; set; }
        public long TargetId { get; set; }
        public string TargetName { get; set; }
        public string ImgPath { get; set; }
        public bool IsRead { get; set; }
    }
}