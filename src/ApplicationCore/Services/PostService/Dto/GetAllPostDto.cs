using System;
using System.Collections.Generic;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.PostVotes;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.PostService.Dto
{
    public class Example
    {
        public List<GetAllPostDto> Posts { get; set; }
    }

    public class GetAllPostDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string MediaContentPath { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long CommentsCount { get; set; }
        public long VoteCount { get; set; }
        public PostVote UserPostVote { get; set; }
        public string LinkUrl { get; set; }
        public PostCommunityDto Community { get; set; }
        public PostUserDto User { get; set; }
    }
}