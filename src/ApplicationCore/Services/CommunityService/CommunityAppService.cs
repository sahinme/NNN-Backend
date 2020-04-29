using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.BlobService;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService
{
    public class CommunityAppService:ICommunityAppService
    {
        private readonly IAsyncRepository<Community> _communityRepository;
        private readonly IBlobService _blobService;

        public CommunityAppService(IAsyncRepository<Community> communityRepository,IBlobService blobService)
        {
            _communityRepository = communityRepository;
            _blobService = blobService;
        }
        
        public async Task<Community> CreateCommunity(CreateCommunity input)
        {
            var model = new Community
            {
                Name = input.Name,
                Description = input.Description
            };
            if (input.LogoFile != null)
            {
                var path = await _blobService.InsertFile(input.LogoFile);
                model.LogoPath = path;
            }
            await _communityRepository.AddAsync(model);
            return model;
        }

        public async Task<List<GetAllCommunityDto>> GetAll()
        {
            var result = await _communityRepository.GetAll().Where(x => x.IsDeleted == false)
                .Include(x => x.Users).ThenInclude(x => x.User).Select(x=> new GetAllCommunityDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    MemberCount = x.Users.Count,
                    LogoPath = BlobService.BlobService.GetImageUrl(x.LogoPath)
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
                    LogoPath = BlobService.BlobService.GetImageUrl(x.LogoPath),
                    Members = x.Users.Where(m=>m.IsDeleted==false).Select(m => new CommunityUserDto
                    {
                        Id = m.User.Id,
                        Username = m.User.Username,
                        ProfileImg = BlobService.BlobService.GetImageUrl(m.User.ProfileImagePath)
                    }).ToList()
                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<GetAllCommunityDto>> GetPopulars()
        {
            var result = await _communityRepository.GetAll().Where(x => x.IsDeleted == false)
                .OrderBy(x=>x.Users.Count).Take(5)
                .Include(x => x.Users).ThenInclude(x => x.User).Select(x=> new GetAllCommunityDto
                {
                    Name = x.Name,
                    Description = x.Description,
                    MemberCount = x.Users.Count,
                    LogoPath = BlobService.BlobService.GetImageUrl(x.LogoPath)
                }).ToListAsync();
            return result;
        }
    }
}