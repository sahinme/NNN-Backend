using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.PostVotes;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.PostService
{
    public interface IPostAppService
    {
        Task<Post> CreatePost(CreatePostDto input);
        Task<PostDto> GetPostById(Guid id,Guid? userId);
        Task Delete(Guid id);
        Task DeleteModerator(ModeratorDeleteDto input);
        Task<List<UserPostsDto>> GetUserPosts(IdOrUsernameDto input);
        Task<List<GetAllPostDto>> HomePosts(Guid userId);
        Task<List<GetAllPostDto>> UnauthorizedHomePosts();
        Task<PostVote> Vote(CreateVoteDto input);
    }
}