using TESNS.Models.Authentication;

namespace TESNS.Services
{
    public interface ISendEmailService
    {
        void SendEmail(string receiverMail,AppUser userr);
    }
}
