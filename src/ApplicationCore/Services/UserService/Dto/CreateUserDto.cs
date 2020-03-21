using Microsoft.AspNetCore.Http;

namespace Nnn.ApplicationCore.Services.UserService.Dto
{
    public class CreateUserDto
    {
        public char Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public IFormFile ProfileImage { get; set; }
       
    }
}