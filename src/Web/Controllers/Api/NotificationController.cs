using System;
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
        
        [HttpGet]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var result = await _notificationAppService.GetUserNotifications(userId);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCount(Guid userId)
        {
            var result = await _notificationAppService.GetUnReads(userId);
            return Ok(new { count=result });
        }

        [HttpPut]
        public async Task<IActionResult> MarkAsRead(Guid[] ids)
        {
            await _notificationAppService.MarkAsRead(ids);
            return Ok();
        }
    }
}