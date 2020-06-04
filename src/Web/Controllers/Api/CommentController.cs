using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.CommentService;
using Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto;
using Microsoft.Nnn.Web.Identity;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class CommentController:BaseApiController
    {
        private readonly ICommentAppService _commentAppService;
        private readonly IAsyncRepository<Comment> _commentRepository;

        public CommentController(ICommentAppService commentAppService,IAsyncRepository<Comment> commentRepository)
        {
            _commentAppService = commentAppService;
            _commentRepository = commentRepository;
        }
        
         [Authorize]
         [HttpPost]
         public async Task<IActionResult> Create(CreateCommentDto input)
         {
             var token = GetToken();
             var userId = LoginHelper.GetClaim(token, "UserId");
             
             input.UserId = Guid.Parse(userId);
             
             var result = await _commentAppService.CreateComment(input);
             return Ok(result);
         }

        [HttpGet]
        public async Task<IActionResult> GetPostComments(Guid postId)
        {
            var comments = await _commentAppService.GetPostComments(postId);
            return Ok(comments);
        }

//        [Authorize]
//        [HttpPut]
//        public async Task<IActionResult> UpdateComment(UpdateComment input)
//        {
//            var createdComment = await _commentAppService.UpdateComment(input);
//            return Ok(createdComment);
//        }
        
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment.UserId != Guid.Parse(userId))
            {
                return Unauthorized();
            }
            await _commentAppService.Delete(id);
            return Ok();
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Like(Guid commentId)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");
            
            var result = await _commentAppService.Like(Guid.Parse(userId), commentId);
            return Ok(result);
        }
        
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Unlike(Guid commentId)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");
            
            await _commentAppService.Unlike(Guid.Parse(userId), commentId);
            return Ok();
        }
    }
}