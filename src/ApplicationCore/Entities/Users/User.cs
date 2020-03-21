using System.ComponentModel.DataAnnotations;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.Users
{
    public class User:BaseEntity,IAggregateRoot
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public char Gender { get; set; }
        [Required]
        [MinLength(6)]
        public string Username { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
    
}