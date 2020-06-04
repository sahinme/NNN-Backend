using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Entities.Replies;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto;
using Microsoft.Nnn.Web.Identity;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class ReplyController:BaseApiController
    {
        private readonly IReplyAppService _replyAppService;
        private readonly IAsyncRepository<Reply> _replyRepository;

        public ReplyController(IReplyAppService replyAppService,IAsyncRepository<Reply> replyRepository )
        {
            _replyAppService = replyAppService;
            _replyRepository = replyRepository;
        }

        [Authorize]
        [HttpPost]
        public async  Task<IActionResult> CreateReply(CreateReplyDto input)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            if (input.UserId != Guid.Parse(userId)) return Unauthorized();
            
            var reply = await _replyAppService.CreateReply(input);
            return Ok(reply);
        }
        
        [Authorize]
        [HttpPost]
        public async  Task<IActionResult> Like(Guid replyId)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            var reply = await _replyAppService.Like(Guid.Parse(userId), replyId);
            return Ok(reply);
        }
        
        [Authorize]
        [HttpDelete]
        public async  Task<IActionResult> Unlike(Guid replyId)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            await _replyAppService.Unlike(Guid.Parse(userId), replyId);
            return Ok();
        }
        
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            var reply = await _replyRepository.GetByIdAsync(id);
            if (reply.UserId != Guid.Parse(userId)) return Unauthorized();
            
            await _replyAppService.Delete(id);
            return Ok();
        }
    }
}