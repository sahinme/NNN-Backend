using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Conversations;
using Microsoft.Nnn.ApplicationCore.Entities.Messages;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.ConversationService;
using Microsoft.Nnn.ApplicationCore.Services.ConversationService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.MessageService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.MessageService
{
    public class MessageAppService:IMessageAppService
    {
        private readonly IAsyncRepository<Message> _messageRepository;
        private readonly IAsyncRepository<Conversation> _conversationRepository;
        private readonly IConversationAppService _conversationAppService;

        public MessageAppService(IAsyncRepository<Message> messageRepository,IConversationAppService conversationAppService,
            IAsyncRepository<Conversation> conversationRepository)
        {
            _messageRepository = messageRepository;
            _conversationAppService = conversationAppService;
            _conversationRepository = conversationRepository;
        }
        
        public async Task<Message> Create(CreateMessageDto input)
        {
          
            if (input.ConversationId == 0)
            {
               var conversation = await _conversationAppService.Create(new CreateConversationDto
                    {ReceiverId = input.ReceiverId, SenderId = input.SenderId});
               
               var message = new Message
               {
                   SenderId = input.SenderId,
                   ReceiverId = input.ReceiverId,
                   Content = input.Content,
                   ConversationId = conversation.Id
               };
               await _messageRepository.AddAsync(message);
               return message;
            }
            var model = new Message
            {
                SenderId = input.SenderId,
                ReceiverId = input.ReceiverId,
                Content = input.Content,
                ConversationId = input.ConversationId
            };
            await _messageRepository.AddAsync(model);
            return model;
        }
    }
}