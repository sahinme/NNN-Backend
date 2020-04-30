using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.CommentService;
using Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class CommentController:BaseApiController
    {
        private readonly ICommentAppService _commentAppService;

        public CommentController(ICommentAppService commentAppService)
        {
            _commentAppService = commentAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPostComments(long postId)
        {
            var comments = await _commentAppService.GetPostComments(postId);
            return Ok(comments);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment(UpdateComment input)
        {
            var createdComment = await _commentAppService.UpdateComment(input);
            return Ok(createdComment);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            await _commentAppService.Delete(id);
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> Like(long userId, long commentId)
        {
            var result = await _commentAppService.Like(userId,commentId);
            return Ok(result);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Unlike(long userId, long commentId)
        {
            await _commentAppService.Unlike(userId,commentId);
            return Ok();
        }
    }
}