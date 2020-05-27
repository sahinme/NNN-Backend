using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Entities.PostVotes;
using Microsoft.Nnn.ApplicationCore.Services.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostService;
using Microsoft.Nnn.ApplicationCore.Services.PostService.Dto;

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

        [HttpGet("by-id")]
        public async Task<IActionResult> Get(Guid id,Guid? userId)
        {
            var post = await _postAppService.GetPostById(id,userId);
            return Ok(post);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserPosts([FromQuery] IdOrUsernameDto input)
        {
            var post = await _postAppService.GetUserPosts(input);
            return Ok(post);
        }
        
        [HttpGet]
        public async Task<IActionResult> HomePosts(Guid userId)
        {
            var post = await _postAppService.HomePosts(userId);
            return Ok(post);
        }
        
        [HttpGet]
        public async Task<IActionResult> PagedHomePosts([FromQuery] PaginationParams input)
        {
            var post = await _postAppService.PagedHomePosts(input);
            return Ok(post);
        }
        
        [HttpGet]
        public async Task<IActionResult>  UnauthorizedHomePost()
        {
            var result = await _postAppService.UnauthorizedHomePosts();
            return Ok(result);
        }
        
        
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
             await _postAppService.Delete(id);
            return Ok();
        }
        
        [HttpDelete]
        public async Task<IActionResult> ModeratorDelete(ModeratorDeleteDto input)
        {
            await _postAppService.DeleteModerator(input);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Vote(CreateVoteDto input)
        {
            var result = await _postAppService.Vote(input);
            return Ok(result);
        }
    }
}