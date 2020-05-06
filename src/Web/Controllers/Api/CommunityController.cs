using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class CommunityController:BaseApiController
    {
        private readonly ICommunityAppService _communityAppService;

        public CommunityController(ICommunityAppService communityAppService)
        {
            _communityAppService = communityAppService;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateCommunity([FromForm] CreateCommunity input)
        {
            var result = await _communityAppService.CreateCommunity(input);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _communityAppService.GetAll();
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _communityAppService.GetById(id);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPopulars(long userId)
        {
            var result = await _communityAppService.GetPopulars(userId);
            return Ok(result);
        }

        
    }
}