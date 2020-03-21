using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Replies;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.ReplyService
{
    public class ReplyAppService:IReplyAppService
    {
        private readonly IAsyncRepository<Reply> _replyRepository;

        public ReplyAppService(IAsyncRepository<Reply> replyRepository)
        {
            _replyRepository = replyRepository;
        }
        
        public async Task<Reply> CreateReply(CreateReplyDto input)
        {
            var reply = new Reply
            {
                Content = input.Content,
                UserId = input.UserId,
                CommentId = input.CommentId
            };
            await _replyRepository.AddAsync(reply);
            return reply;
        }
    }
}