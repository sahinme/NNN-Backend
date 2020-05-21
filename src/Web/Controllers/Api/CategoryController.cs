using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.CategoryService;
using Microsoft.Nnn.ApplicationCore.Services.CategoryService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class CategoryController:BaseApiController
    {
        private readonly ICategoryAppService _categoryAppService;

        public CategoryController(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto input)
        {
            var result = await _categoryAppService.CreateCategory(input);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryAppService.GetAllCategories();
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCommunities(Guid categoryId,Guid? userId)
        {
            var result = await _categoryAppService.GetCommunitiesByCategory(categoryId,userId);
            return Ok(result);
        }
    }
}