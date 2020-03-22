namespace Nnn.ApplicationCore.Services.UserService.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        public char Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string ProfileImagePath { get; set; }
    }
}