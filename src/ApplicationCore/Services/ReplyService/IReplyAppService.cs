using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Replies;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.ReplyService
{
    public interface IReplyAppService
    {
        Task<Reply> CreateReply(CreateReplyDto input);
    }
}