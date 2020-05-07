namespace Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto
{
    public class CreateVoteDto
    {
        public long UserId { get; set; }
        public long PostId { get; set; }
        public sbyte Value { get; set; }
    }
}