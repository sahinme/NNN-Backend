using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.Dto;

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
        public async Task<IActionResult> OfModerators(Guid userId)
        {
            var result = await _communityAppService.OfModerators(userId);
            return Ok(result);
        }
        
        [HttpGet("by-id")]
        public async Task<IActionResult> Get(Guid id,Guid userId)
        {
            var result = await _communityAppService.GetById(id,userId);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PaginationParams input)
        {
            var result = await _communityAppService.GetPosts(input);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPopulars(Guid userId)
        {
            var result = await _communityAppService.GetPopulars(userId);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> Search(string text)
        {
            var result = await _communityAppService.Search(text);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> Users(Guid id)
        {
            var result = await _communityAppService.Users(id);
            return Ok(result);
        }
        
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateCommunity input)
        {
            var result = await _communityAppService.Update(input);
            return Ok(result);
        }

        
    }
}