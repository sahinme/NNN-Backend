using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Likes;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.Unlikes;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.PostService
{
    public interface IPostAppService
    {
        Task<Post> CreatePost(CreatePostDto input);
        Task<PostDto> GetPostById(long id,long? userId);
        Task Delete(long id);
        Task<List<UserPostsDto>> GetUserPosts(IdOrUsernameDto input);
        Task<PostLike> LikePost(CreateLikeDto input);
        Task ConvertLike(long id);
        Task<Unlike> UnlikePost(CreateLikeDto input);
        Task<Example> HomePosts(long userId);
        Task<List<GetAllPostDto>> UnauthorizedHomePosts();
    }
}