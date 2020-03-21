using System;

namespace Microsoft.Nnn.ApplicationCore.Services.AnswerService.Dto
{
    public class AnswerDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}