using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.LikeService;
using Microsoft.Nnn.ApplicationCore.Services.LikeService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class LikeController:BaseApiController
    {
        private readonly ILikeAppService _likeAppService;

        public LikeController(ILikeAppService likeAppService)
        {
            _likeAppService = likeAppService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLike(CreateLikeDto input)
        {
            var result = await _likeAppService.CreateLike(input);
            return Ok(result);
        }
    }
}