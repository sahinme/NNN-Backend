using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Likes;
using Microsoft.Nnn.ApplicationCore.Services.LikeService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.LikeService
{
    public interface ILikeAppService
    {
        Task<Like> CreateLike(CreateLikeDto input);
    }
}