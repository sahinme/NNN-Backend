namespace Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto
{
    public class CreateReplyDto
    {
        public string Content { get; set; }
        public long UserId { get; set; }
        public long CommentId { get; set; }
    }
}