using System;

namespace Microsoft.Nnn.ApplicationCore.Services.MessageService.Dto
{
    public class MessageDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public MessageUserDto User { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class MessageUserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string LogoPath { get; set; }
    }
}