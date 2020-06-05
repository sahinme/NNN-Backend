using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.Dto;
using Microsoft.Nnn.Web.Identity;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class CommunityController:BaseApiController
    {
        private readonly ICommunityAppService _communityAppService;

        public CommunityController(ICommunityAppService communityAppService)
        {
            _communityAppService = communityAppService;
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCommunity([FromForm] CreateCommunity input)
        {
            var token = GetToken();
            var userType = LoginHelper.GetClaim(token, "UserRole");
            if (userType != "Admin") return Unauthorized();
            
            var result = await _communityAppService.CreateCommunity(input);
            return Ok(result);
        }
        
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _communityAppService.GetAll();
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> OfModerators()
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");
            
            var result = await _communityAppService.OfModerators(Guid.Parse(userId));
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(string slug)
        {
            var token = GetToken();
            string userId = null;
            bool isTokenEmpty = String.IsNullOrEmpty(token);
            if ( !isTokenEmpty )
            {
                 userId = LoginHelper.GetClaim(token, "UserId");

            }
            
            var result = await _communityAppService.GetById(slug,userId == null ? Guid.Empty : Guid.Parse(userId));
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PageDtoCommunity input)
        {
            var token = GetToken();
            string userId = null;
            bool isTokenEmpty = String.IsNullOrEmpty(token);
            if ( !isTokenEmpty )
            {
                userId = LoginHelper.GetClaim(token, "UserId");

            }

            if (userId != null) input.UserId = Guid.Parse(userId);
            
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
        public async Task<IActionResult> Users(string slug)
        {
            var result = await _communityAppService.Users(slug);
            return Ok(result);
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateCommunity input)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId)) return Unauthorized();
            
            input.ModeratorId = Guid.Parse(userId);
            
            var result = await _communityAppService.Update(input);
            return Ok(result);
        }

        
    }
}