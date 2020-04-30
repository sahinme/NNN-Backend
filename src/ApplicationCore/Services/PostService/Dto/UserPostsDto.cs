using System;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;

namespace Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto
{
    public class UserPostsDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string MediaContentPath { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long CommentsCount { get; set; }
        public long LikesCount { get; set; }
        public long UnlikesCount { get; set; }
        public PostCommunityDto Community { get; set; }
    }

    public class PostCommunityDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}