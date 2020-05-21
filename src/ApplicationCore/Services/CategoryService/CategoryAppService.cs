using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Categories;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.CategoryService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CategoryService
{
    public class CategoryAppService:ICategoryAppService
    {
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IAsyncRepository<Community> _communityRepository;

        public CategoryAppService(IAsyncRepository<Category> categoryRepository,IAsyncRepository<Community> communityRepository)
        {    
            _categoryRepository = categoryRepository;
            _communityRepository = communityRepository;
        }
        
        public async Task<Category> CreateCategory(CreateCategoryDto input)
        {
            var category = new Category
            {
                DisplayName = input.DisplayName
            };
            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            var result = await _categoryRepository.GetAll().Where(x => x.IsDeleted == false).Select(x => new CategoryDto
            {
                Id = x.Id,
                DisplayName = x.DisplayName
            }).ToListAsync();
            return result;
        }

        public async Task<List<GetAllCommunityDto>> GetCommunitiesByCategory(Guid categoryId,Guid? userId)
        {
            var communities = await _communityRepository.GetAll()
                .Where(x => x.IsDeleted == false && x.CategoryId == categoryId)
                .Include(x=>x.Category)
                .Include(x => x.Users).Select(x => new GetAllCommunityDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Category = new CategoryDto
                    {
                        Id = x.Category.Id,
                        DisplayName = x.Category.DisplayName
                    },
                    LogoPath = BlobService.BlobService.GetImageUrl(x.LogoPath),
                    MemberCount = x.Users.Count(m => m.IsDeleted==false),
                    IsUserJoined = x.Users.Any(u => u.IsDeleted == false && u.UserId == userId)
                }).ToListAsync();
            return communities;
        }
    }
}