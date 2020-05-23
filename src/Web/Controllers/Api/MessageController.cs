using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.MessageService;
using Microsoft.Nnn.ApplicationCore.Services.MessageService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class MessageController:BaseApiController
    {
        private readonly IMessageAppService _messageAppService;
        
        public  MessageController(IMessageAppService messageAppService)
        {
            _messageAppService = messageAppService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateMessageDto input)
        {
            var result = await _messageAppService.Create(input);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUnReads(Guid userId)
        {
            var count = await _messageAppService.GetUnreadCount(userId);
            return Ok( new {error=false,count});
        }
        
        [HttpPut]
        public async Task<IActionResult> MarkAsRead(Guid[] ids)
        {
            await _messageAppService.MarkAsRead(ids);
            return Ok();
        }
    }
}