namespace Microsoft.Nnn.ApplicationCore.Services.MessageService.Dto
{
    public class CreateMessageDto
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public long ConversationId { get; set; }
        public string Content { get; set; }
    }
}