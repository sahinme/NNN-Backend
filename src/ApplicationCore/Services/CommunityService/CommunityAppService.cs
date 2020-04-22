using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService
{
    public class CommunityAppService:ICommunityAppService
    {
        private readonly IAsyncRepository<Community> _communityRepository;

        public CommunityAppService(IAsyncRepository<Community> communityRepository)
        {
            _communityRepository = communityRepository;
        }
        
        public async Task<Community> CreateCommunity(CreateCommunity input)
        {
            var model = new Community
            {
                Name = input.Name,
                Description = input.Description
            };
            await _communityRepository.AddAsync(model);
            return model;
        }

        public async Task<List<GetAllCommunityDto>> GetAll()
        {
            var result = await _communityRepository.GetAll().Where(x => x.IsDeleted == false)
                .Include(x => x.Users).ThenInclude(x => x.User).Select(x=> new GetAllCommunityDto
                {
                    Name = x.Name,
                    Description = x.Description,
                    MemberCount = x.Users.Count
                }).ToListAsync();
            return result;
        }

        public async Task<CommunityDto> GetById(long id)
        {
            var result = await _communityRepository.GetAll().Where(x => x.Id == id && x.IsDeleted == false)
                .Include(x => x.Users)
                .ThenInclude(x => x.User).Select(x => new CommunityDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Members = x.Users.Select(m => new CommunityUserDto
                    {
                        Id = m.User.Id,
                        Username = m.User.Username,
                        ProfileImg = BlobService.BlobService.GetImageUrl(m.User.ProfileImagePath)
                    }).ToList()
                }).FirstOrDefaultAsync();
            return result;
        }
    }
}