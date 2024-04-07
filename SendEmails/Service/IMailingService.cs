namespace SendEmails.Service
{
    public interface IMailingService
    {
        Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attchments = null);  
    }
}
