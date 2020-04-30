using System;

namespace Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto
{
    public class ReplyDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public ReplyUserDto ReplyUserInfo { get; set; }
        public long LikeCount { get; set; }
    }

    public class ReplyUserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }
    }
}