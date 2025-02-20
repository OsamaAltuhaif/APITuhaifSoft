namespace TuhaifSoftAPI.EmilSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string message);
    }
}
