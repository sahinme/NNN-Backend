using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.SuggestionService;
using Microsoft.Nnn.Web.Identity;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class AdminController:BaseApiController
    {
        private readonly ISuggestionAppService _suggestionAppService;
        private readonly IUserService _userAppService;
        private readonly IAsyncRepository<User> _userRepo;

        public AdminController(ISuggestionAppService suggestionAppService,IUserService userAppService,
            IAsyncRepository<User> userRepo)
        {
            _suggestionAppService = suggestionAppService;
            _userAppService = userAppService;
            _userRepo = userRepo;
        }

        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Suggestions(Guid userId)
        {
            var token = GetToken();
            var userType = LoginHelper.GetClaim(token, "UserRole");
            if (userType != "Admin") return Unauthorized();
            
            var user = await _userRepo.GetByIdAsync(userId);
            if (!user.IsAdmin)
            {
                return Unauthorized();
            }

            var result = await _suggestionAppService.GetAll();
            return Ok(result);
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Gul()
        {
            var token = GetToken();
            var userType = LoginHelper.GetClaim(token, "UserRole");
            if (userType != "Admin") return Unauthorized();
            
            var users = await _userRepo.GetAll().ToListAsync();
            return Ok(users);
        }
        
        
    }
}