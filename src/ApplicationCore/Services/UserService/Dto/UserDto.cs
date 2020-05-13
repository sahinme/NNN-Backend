namespace Nnn.ApplicationCore.Services.UserService.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string ProfileImagePath { get; set; }
        public char Gender { get; set; }
        public bool IsModerator { get; set; }
        public string Bio { get; set; }
    }
}