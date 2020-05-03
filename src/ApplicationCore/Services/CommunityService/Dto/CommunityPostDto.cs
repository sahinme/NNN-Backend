using System;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto
{
    public class CommunityPostDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string MediaContentPath { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long CommentsCount { get; set; }
        public long LikesCount { get; set; }
        public long UnlikesCount { get; set; }
        public PostUserDto User { get; set; }
    }
}