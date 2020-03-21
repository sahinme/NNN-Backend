namespace Nnn.ApplicationCore.Services.UserService.Dto
{
    public class UpdateUserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
                        
        public string Surname { get; set; }
                        
        public char Gender { get; set; }
          
        public int Age { get; set; }
                        
          //meslek
        public string Profession { get; set; }
                
        public string Username { get; set; }
                
        public string EmailAddress { get; set; }
                
        public string PhoneNumber { get; set; }
    }
}