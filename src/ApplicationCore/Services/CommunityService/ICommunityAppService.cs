using System;
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
        Task<CommunityDto> GetById(Guid id,Guid? userId);
        Task<List<GetAllCommunityDto>> GetPopulars(Guid? userId);
        Task<List<GetAllCommunityDto>> OfModerators(Guid userId);
        Task<List<CommunityUserDto>> Users(Guid id);
        Task<List<SearchDto>> Search(string text);
    }
}