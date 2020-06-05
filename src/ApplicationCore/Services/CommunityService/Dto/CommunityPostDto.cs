using System;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.PostVotes;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto
{
    public class CommunityPostDto
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string MediaContentPath { get; set; }
        public ContentType ContentType { get; set; }
        public int PageNumber { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LinkUrl { get; set; }
        public long CommentsCount { get; set; }
        public PostVote UserPostVote { get; set; }
        public long VoteCount { get; set; }
        public PostUserDto User { get; set; }
    }
}