using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostService;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class PostController:BaseApiController
    {
        private readonly IPostAppService _postAppService;

        public PostController(IPostAppService postAppService)
        {
            _postAppService = postAppService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto input)
        {
            var createdPost = await _postAppService.CreatePost(input);
            return Ok(createdPost);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostById(long id)
        {
            var post = await _postAppService.GetPostById(id);
            return Ok(post);
        }
    }
}