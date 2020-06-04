using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.SuggestionService;
using Microsoft.Nnn.ApplicationCore.Services.SuggestionService.Dto;
using Microsoft.Nnn.Web.Identity;
using Nnn.ApplicationCore.Services.UserService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class UserController:BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly ISuggestionAppService _suggestionAppService;

        public UserController(IUserService userService,
                IEmailSender emailSender,ISuggestionAppService suggestionAppService
            )
        {
            _userService = userService;
            _emailSender = emailSender;
            _suggestionAppService = suggestionAppService;
        }

        [HttpGet("by-id")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(string username)
        {
            var result = await _userService.GetByUsername(username);
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Suggestion(SuggestionCreate input)
        {
            var result = await _suggestionAppService.Create(input);
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm]  CreateUserDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var user = await _userService.CreateUser(input);
                if (user.Id != Guid.Empty)
                {
                    var subject = "https://saalla.com/#/verify/" + user.VerificationCode;
                    await _emailSender.SendEmail(user.EmailAddress, "E-posta onaylama", subject);
                }

                user.Password = null;
                user.VerificationCode = null;
                if (user.Id == Guid.Empty)
                    return Ok(new
                        {user.EmailAddress,user.Username, error = true});
                
                return Ok( new {success=true});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok(new {message = e});
                throw;
            }
           
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> JoinCommunity(string slug)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            
            var result = await _userService.JoinCommunity(Guid.Parse(userId), slug);
            return Ok(result);
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserCommunities()
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");
            
            var result = await _userService.GetUserCommunities(Guid.Parse(userId));
            return Ok(result);
        }
        
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> LeaveFromCommunity(string slug)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");
            
            await _userService.LeaveFromCommunity(Guid.Parse(userId), slug);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> ModeratorRejectedJoin(ModeratorRejected input)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            if (input.ModeratorId != Guid.Parse(userId)) return Unauthorized();
            
            await _userService.ModeratorRejectedJoin(input);
            return Ok();
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto input)
        {
            var token = GetToken();
            var userId = LoginHelper.GetClaim(token, "UserId");

            if (string.IsNullOrEmpty(token)) return Unauthorized();
            input.Id = Guid.Parse(userId);
            await _userService.UpdateUser(input);
            return Ok();
        }

//        [Authorize]
//        [HttpDelete]
//        public async Task<IActionResult> DeleteUser(Guid id)
//        {
//            await _userService.DeleteUser(id);
//            return Ok();
//        }

//        [HttpPost]
//        public async Task EmailSend(string email, string subject, string message)
//        {
//            await _emailSender.SendEmail(email, subject, message);
//        }
    }
}