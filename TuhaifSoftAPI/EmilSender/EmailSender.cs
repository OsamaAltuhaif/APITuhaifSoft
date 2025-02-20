using MimeKit;
using System.Net;
using MailKit.Net.Smtp;

namespace TuhaifSoftAPI.EmilSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _fromEmail;
        private readonly string _password;

        public EmailSender(IConfiguration configuration)
        {
            _config = configuration;
           /* var emailSettings = configuration.GetSection("EmailSettings");
            _smtpServer = emailSettings["SmtpServer"];
            _port = int.Parse(emailSettings["Port"]);
            _fromEmail = emailSettings["FromEmail"];
            _password = emailSettings["Password"];*/
        }

        public async Task SendEmailAsync(string email, string verifaction)
        {
            var emailSetting = _config.GetSection("EmailSettings");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TuhaifSoftAPI", emailSetting["FromEmail"]));
            message.To.Add(new MailboxAddress("User", email));
            message.Subject = "تفعيل الحساب";

            var activationLink = $"https://localhost:7013/verify-email?email={email}&code={verifaction}";
            message.Body = new TextPart("html")
            {
                Text = $"<h1> hellow!</h1><p>click please<a href='{activationLink}'> here</a> to active your account. </p>"
            };
            using var client = new SmtpClient();
            await client.ConnectAsync(emailSetting["SmtpServer"], int.Parse(emailSetting["Port"]), true);
            await client.AuthenticateAsync(emailSetting["FromEmail"], emailSetting["Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

}
