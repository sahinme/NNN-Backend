using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Services.NotificationService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.NotificationService
{
    public interface INotificationAppService
    {
        Task CreateNotify(CreateNotificationDto input);
        Task<bool> MarkAsRead(long id);
    }
}