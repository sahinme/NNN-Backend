using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.ConversationService;
using Microsoft.Nnn.ApplicationCore.Services.ConversationService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class ConversationController:BaseApiController
    {
        private readonly IConversationAppService _conversationAppService;
        
        public  ConversationController(IConversationAppService conversationAppService)
        {
            _conversationAppService = conversationAppService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateConversationDto input)
        {
            var result = await _conversationAppService.Create(input);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll(long userId)
        {
            var result = await _conversationAppService.GetAll(userId);
            return Ok(result);
        }
        
        [HttpGet("by-id")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _conversationAppService.GetById(id);
            return Ok(result);
        }
    }
}