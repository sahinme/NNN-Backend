using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService
{
    public interface ICommunityAppService
    {
        Task<Community> CreateCommunity(CreateCommunity input);
        Task<List<GetAllCommunityDto>> GetAll();
        Task<Community> Update(UpdateCommunity input);
        Task<CommunityDto> GetById(long id,long? userId);
        Task<List<GetAllCommunityDto>> GetPopulars(long? userId);
        Task<List<GetAllCommunityDto>> OfModerators(long userId);
        Task<List<CommunityUserDto>> Users(long id);
    }
}