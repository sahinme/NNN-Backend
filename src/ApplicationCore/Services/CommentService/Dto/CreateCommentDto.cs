namespace Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto
{
    public class CreateCommentDto
    {
        public long UserId { get; set; }
        public long PostId { get; set; }
        public string Content { get; set; }
    }
}