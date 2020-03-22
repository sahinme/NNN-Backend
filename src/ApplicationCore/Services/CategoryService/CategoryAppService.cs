using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Categories;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.CategoryService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CategoryService
{
    public class CategoryAppService:ICategoryAppService
    {
        private readonly IAsyncRepository<Category> _categoryRepository;

        public CategoryAppService(IAsyncRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
    }
}