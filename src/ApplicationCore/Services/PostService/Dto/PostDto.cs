using System;
using System.Collections.Generic;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Services.CategoryService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto
{
    public class PostDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string ContentPath { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long LikeCount { get; set; }
        public long UnlikeCount { get; set; }
        public PostCommunityDto Community { get; set; }
        public PostUserDto UserInfo { get; set; }
        public List<CommentDto> Comments { get; set; }
       
    }

    public class PostUserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }
    }
}