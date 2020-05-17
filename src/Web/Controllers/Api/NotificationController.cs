using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Nnn.ApplicationCore.Services.NotificationService;
using Microsoft.Nnn.ApplicationCore.Services.NotificationService.Dto;

namespace Microsoft.Nnn.Web.Controllers.Api
{
    public class NotificationController:BaseApiController
    {
        private readonly INotificationAppService _notificationAppService;

        public NotificationController(INotificationAppService notificationAppService)
        {
            _notificationAppService = notificationAppService;
        }
        
//        [HttpPost]
//        public async Task<IActionResult> CreateNotify(CreateNotificationDto input)
//        {
//            await _notificationAppService.CreateNotify(input);
//            return Ok();
//        }
//        
//        [HttpPut]
//        public async Task<IActionResult> MarkAsRead(long id)
//        {
//            var result = await _notificationAppService.MarkAsRead(id);
//            return Ok(result);
//        }
    }
}