using System;
using System.Collections.Generic;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;

namespace Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto
{
    public class GetAllPostDto
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
        public PostUserDto User { get; set; }
    }

    public class Example
    {
        public List<GetAllPostDto> Data { get; set; }
    }
    
}