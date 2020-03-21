using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.PostService
{
    public class PostAppService:IPostAppService
    {
        private readonly IAsyncRepository<Post> _postRepository;

        public PostAppService(IAsyncRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }
        
        public async Task<Post> CreatePost(CreatePostDto input)
        {
            var post = new Post
            {
                Title = input.Title,
                Where = input.Where,
                Why = input.Why,
                How = input.How,
                UserId = input.UserId
            };
            await _postRepository.AddAsync(post);
            return post;
        }

        public async Task<PostDto> GetPostById(long id)
        {
            var post = await _postRepository.GetAll().Where(x => x.Id == id).Include(x => x.User)
                .Select(x => new PostDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Where = x.Where,
                    Why = x.Why,
                    How = x.How,
                    CreatedDateTime = x.CreatedDate,
                    UserInfo = new PostUserDto
                    {
                        Id = x.User.Id,
                        UserName = x.User.Username,
                        ProfileImagePath = x.User.ProfileImagePath
                    }
                }).FirstOrDefaultAsync();
            return post;
        }
    }
}