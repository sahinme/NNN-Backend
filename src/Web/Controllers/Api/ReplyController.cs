using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class ReplyController:BaseApiController
    {
        private readonly IReplyAppService _replyAppService;

        public ReplyController(IReplyAppService replyAppService)
        {
            _replyAppService = replyAppService;
        }

        [HttpPost]
        public async  Task<IActionResult> CreateReply(CreateReplyDto input)
        {
            var reply = await _replyAppService.CreateReply(input);
            return Ok(reply);
        }
    }
}