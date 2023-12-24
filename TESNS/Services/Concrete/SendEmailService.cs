using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using TESNS.Models;
using TESNS.Models.Authentication;

namespace TESNS.Services.Concrete
{

    public class SendEmailService : ISendEmailService
    {
        private readonly ApplicationDbContext _context;
        public SendEmailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SendEmail(string receiverMail, AppUser userr)
        {
            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddressFrom = new MailboxAddress("SNS", "emirdeveci55@gmail.com");
            MailboxAddress mailboxAddressTo = new MailboxAddress("User", receiverMail);
            mimeMessage.From.Add(mailboxAddressFrom);
            mimeMessage.To.Add(mailboxAddressTo);
            var bodyBuilder = new BodyBuilder();
            Random rnd = new Random();
            var random = rnd.Next(10000, 100000);
            var randomm = System.Convert.ToString(random);
            userr.PasswordResetToken = randomm;
            //user.PasswordResetToken = CreateRandomToken();
            _context.SaveChanges();
            bodyBuilder.TextBody = "Your code :" + userr.PasswordResetToken;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = "Your Smiley Code";
            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("emirdeveci55@gmail.com", "pvqqmckeejfvppnq");
            client.Send(mimeMessage);
            client.Disconnect(true);
        }
    }
}
