using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.PostService
{
    public interface IPostAppService
    {
        Task<Post> CreatePost(CreatePostDto input);
        Task<PostDto> GetPostById(long id);
    }
}