using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Nnn.ApplicationCore.Services.UserService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class UserController:BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public UserController(IUserService userService,
                IEmailSender emailSender
            )
        {
            _userService = userService;
            _emailSender = emailSender;
        }

        [HttpGet("by-id")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var result = await _userService.GetByUsername(username);
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
                var subject = "http:saalla.com/" + user.VerificationCode;
                await _emailSender.SendEmailAsync(user.EmailAddress, "E-posta onaylama",subject );
                user.Password = null;
                user.VerificationCode = null;
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok(new {message = e});
                throw;
            }
           
        }
        
        [HttpPost]
        public async Task<IActionResult> JoinCommunity(Guid userId,Guid communityId)
        {
            var result = await _userService.JoinCommunity(userId, communityId);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserCommunities(Guid userId)
        {
            var result = await _userService.GetUserCommunities(userId);
            return Ok(result);
        }
        
        [HttpDelete]
        public async Task<IActionResult> LeaveFromCommunity(Guid userId,Guid communityId)
        {
            await _userService.LeaveFromCommunity(userId, communityId);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> ModeratorRejectedJoin(ModeratorRejected input)
        {
            await _userService.ModeratorRejectedJoin(input);
            return Ok();
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto input)
        {
            await _userService.UpdateUser(input);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }

        [HttpPost]
        public async Task EmailSend(string email, string subject, string message)
        {
            await _emailSender.SendEmailAsync(email, subject, message);
        }
    }
}