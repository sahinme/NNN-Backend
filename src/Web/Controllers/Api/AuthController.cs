using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Nnn.ApplicationCore.Services.UserService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class AuthController:BaseApiController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> SendResetCode(string emailAddress)
        {
            var result = await _userService.SendResetCode(emailAddress);
            return Ok(new {status = result});
        }
        
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto input)
        {
            var result = await _userService.ResetPassword(input);
            return Ok(new {status = result});
        }
        
        [HttpPut]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto input)
        {
            var result = await _userService.ChangePassword(input);
            return Ok(new {status = result});
        }
        
    }
}