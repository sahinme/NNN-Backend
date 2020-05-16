using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Conversations;
using Microsoft.Nnn.ApplicationCore.Services.ConversationService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.ConversationService
{
    public interface IConversationAppService
    {
        Task<Conversation> Create(CreateConversationDto input);
        Task<List<ConversationDto>> GetAll(long userId);
        Task<ConversationDto> GetById(long id);
        Task Delete(long id);
    }
}