namespace Microsoft.Nnn.ApplicationCore.Services.NotificationService.Dto
{
    public class CreateNotificationDto
    {
        public string Title { get; set; }
        public long SenderId { get; set; }
        public long ContentId { get; set; }
        public string Content { get; set; }
        public long OwnerId { get; set; }
        public bool IsRead { get; set; }
    }
}