using Microsoft.AspNetCore.Http;

namespace Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto
{
    public class CreatePostDto
    {
        public string Content { get; set; }
        public IFormFile ContentFile { get; set; }
        public long CommunityId { get; set; }
        public long UserId { get; set; }
        //public string[] Tags { get; set; }
    }
}