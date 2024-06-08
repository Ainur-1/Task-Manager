using MailKit.Net.Smtp;
using MimeKit;

namespace Task_Manager.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("your-email@example.com", "Your Name"));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.example.com", 587, false);
                await client.AuthenticateAsync("username", "password");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
