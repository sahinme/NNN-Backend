using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Nnn.ApplicationCore.Services.UserService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class TokenController:BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
      

        public TokenController(IConfiguration configuration,IUserService userService
        )
        {
            _configuration = configuration;
            _userService = userService;
          
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult>  Post([FromBody]LoginDto request)
        {
            if (ModelState.IsValid)
            {
                var isUserValid = await _userService.Login(request); 
                if (!isUserValid)
                {
                    return NotFound();
                }
 
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
 
                var token = new JwtSecurityToken
                (
                    issuer: _configuration["Issuer"], 
                    audience: _configuration["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(30), // 30 gün geçerli olacak
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SigningKey"])),//appsettings.json içerisinde bulunan signingkey değeri
                        SecurityAlgorithms.HmacSha256)
                );

                var userData = await _userService.GetByUsername(request.Username);
                
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token),success=true,user=userData });
            }
            return BadRequest();
        }
    }
}