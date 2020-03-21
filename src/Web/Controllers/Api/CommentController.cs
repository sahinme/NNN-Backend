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

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto input)
        {
            var createdComment = await _commentAppService.CreateComment(input);
            return Ok(createdComment);
        }
    }
}