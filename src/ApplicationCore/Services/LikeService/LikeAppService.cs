using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Likes;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.LikeService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.LikeService
{
    public class LikeAppService:ILikeAppService
    {
        private readonly IAsyncRepository<Like> _likeRepository;

        public LikeAppService(IAsyncRepository<Like> likeRepository)
        {
            _likeRepository = likeRepository;
        }
        
        public async Task<Like> CreateLike(CreateLikeDto input)
        {
            var model = new Like
            {
                UserId = input.UserId,
                EntityType = input.EntityType,
                EntityId = input.EntityId
            };
            await _likeRepository.AddAsync(model);
            return model;
        }
    }
}