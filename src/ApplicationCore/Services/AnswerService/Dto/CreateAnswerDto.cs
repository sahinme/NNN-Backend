
namespace Microsoft.Nnn.ApplicationCore.Services.AnswerService.Dto
{
    public class CreateAnswerDto
    {
        public string Description { get; set; }
        public long QuestionId { get; set; }
        public long EntityId { get; set; }
      
    }
}