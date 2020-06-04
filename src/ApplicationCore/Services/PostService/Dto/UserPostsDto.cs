using System;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.PostVotes;

namespace Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto
{
    public class UserPostsDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string MediaContentPath { get; set; }
        public string LinkUrl { get; set; }
        public ContentType ContentType { get; set; }    
        public DateTime CreatedDateTime { get; set; }
        public long CommentsCount { get; set; }
        public long VoteCount { get; set; }
        public PostVote UserPostVote { get; set; }
        public PostCommunityDto Community { get; set; }
    }

    public class PostCommunityDto
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
    }
}