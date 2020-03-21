namespace Microsoft.Nnn.ApplicationCore.Services.NotificationService.Dto
{
    public class NotificationDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long ContentId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
    }
}