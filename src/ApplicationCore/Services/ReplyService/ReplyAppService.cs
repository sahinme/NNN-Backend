using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Replies;
using Microsoft.Nnn.ApplicationCore.Entities.ReplyLikes;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.ReplyService
{
    public class ReplyAppService:IReplyAppService
    {
        private readonly IAsyncRepository<Reply> _replyRepository;
        private readonly IAsyncRepository<ReplyLike> _likeRepository;

        public ReplyAppService(IAsyncRepository<Reply> replyRepository, IAsyncRepository<ReplyLike> likeRepository)
        {
            _replyRepository = replyRepository;
            _likeRepository = likeRepository;
        }
        
        public async Task<Reply> CreateReply(CreateReplyDto input)
        {
            var reply = new Reply
            {
                Content = input.Content,
                UserId = input.UserId,
                CommentId = input.CommentId
            };
            if (input.ParentId != null)
            {
                reply.ParentId = input.ParentId;
            }
            await _replyRepository.AddAsync(reply);
            return reply;
        }

        public async Task<ReplyLike> Like(long userId, long replyId)
        {
            var isExist = await _likeRepository.GetAll().Where(x => x.UserId == userId && x.ReplyId == replyId && x.IsDeleted==false)
                .FirstOrDefaultAsync();
            if (isExist != null)
            {
                throw new Exception("Bu islem zaten yapilmis");
            }
            
            var model = new ReplyLike
            {
                UserId = userId,
                ReplyId = replyId
            };
            await _likeRepository.AddAsync(model);
            return model;
        }

        public async Task Unlike(long userId, long replyId)
        {
            var like = await _likeRepository.GetAll().Where(x => x.UserId == userId && x.ReplyId == replyId && x.IsDeleted==false )
                .FirstOrDefaultAsync();
            like.IsDeleted = true;
            await _likeRepository.UpdateAsync(like);
        }
        
        public async Task Delete(long id)
        {
            var comment = await _replyRepository.GetByIdAsync(id);
            comment.IsDeleted = true;
            await _replyRepository.UpdateAsync(comment);
        }
    }
}