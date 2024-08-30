namespace RYT.Services.Emailing
{
    public interface IEmailService
    {
        Task<string> SendEmailAsync(string recipientEmail, string subject, string body);
    }
}
