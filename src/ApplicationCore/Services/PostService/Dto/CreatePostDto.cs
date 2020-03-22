namespace Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Why { get; set; }
        public string How { get; set; }
        public string Where { get; set; }
        public long UserId { get; set; }
        public long[] CategoryIds { get; set; }
        public string[] Tags { get; set; }
    }
}