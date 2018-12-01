using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using EleksTask;

namespace TourServer
{
    public class EmailService : IEmailService
    {
        public async Task SendConfirmLetter(string token, string userId, string userEmail)
        {
            var apiPath = "http://localhost:4200/confirmEmail?token=" + token + "&user=" + userId;
            var link = "<a href='" + apiPath + "'>link</a>";

            await SendEmailAsync(userEmail, "Confirm Email", link);
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Адміністрація сайту", "ivan.kiselichnik@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("ivan.kiselichnik@gmail.com", "56Comanche");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}

