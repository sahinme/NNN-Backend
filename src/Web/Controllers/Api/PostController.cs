using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.PostVotes;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostService;
using Microsoft.Nnn.ApplicationCore.Services.PostService.Dto;
using Microsoft.Nnn.Web.Identity;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class PostController:BaseApiController
    {
        private readonly IPostAppService _postAppService;
        private readonly IAsyncRepository<Post> _postRepository;

        public PostController(IPostAppService postAppService, IAsyncRepository<Post> postRepository )
        {
            _postAppService = postAppService;
            _postRepository = postRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto input)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            if (userId==null) return Unauthorized();

            input.UserId = Guid.Parse(userId);
            
            var createdPost = await _postAppService.CreatePost(input);
            return Ok(createdPost);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid status)
        {
            var token = GetToken();
            string userId = null;
            if (!string.IsNullOrEmpty(token))
            {  userId = LoginHelper.GetClaim(token, "UserId");
            }
            
            var post = await _postAppService.GetPostById(status,Guid.Parse(userId));
            return Ok(post);
        }
        
        
        [HttpGet]
        public async Task<IActionResult> GetUserPosts([FromQuery] IdOrUsernameDto input)
        {
            var token = GetToken();
            if (token != "null")
            {
                var userId = LoginHelper.GetClaim(token, "UserId");
                input.Id = Guid.Parse(userId);
            }

            if (input.Id == null) input.Id = Guid.Empty;
           
            var post = await _postAppService.GetUserPosts(input);
            return Ok(post);
        }
        
//        [HttpGet]
//        public async Task<IActionResult> HomePosts(Guid userId)
//        {
//            var post = await _postAppService.HomePosts(userId);
//            return Ok(post);
//        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> PagedHomePosts([FromQuery] PaginationParams input)
        {
            var post = await _postAppService.PagedHomePosts(input);
            return Ok(post);
        }
        
        [HttpGet]
        public async Task<IActionResult> PagedUnauthorizedHomePost([FromQuery] PaginationParams input)
        {
            var post = await _postAppService.PagedUnauthorizedHomePosts(input);
            return Ok(post);
        }
        
//        [HttpGet]
//        public async Task<IActionResult>  UnauthorizedHomePost()
//        {
//            var result = await _postAppService.UnauthorizedHomePosts();
//            return Ok(result);
//        }
        
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            var post = await _postRepository.GetByIdAsync(id);
            
            if(post.UserId != Guid.Parse(userId)) return Unauthorized();
            
            await _postAppService.Delete(id);
            return Ok();
        }
        
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> ModeratorDelete(ModeratorDeleteDto input)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            if (input.ModeratorId != Guid.Parse(userId)) return Unauthorized();
            
            await _postAppService.DeleteModerator(input);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Vote(CreateVoteDto input)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            if (input.UserId != Guid.Parse(userId)) return Unauthorized();
            
            var result = await _postAppService.Vote(input);
            return Ok(result);
        }
    }
}