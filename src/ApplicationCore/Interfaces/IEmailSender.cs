using System.Threading.Tasks;

namespace Microsoft.Nnn.ApplicationCore.Interfaces
{

    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
