using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendEmails.Dtos;
using SendEmails.Service;
using System.Threading.Tasks;

namespace SendEmails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailingController : ControllerBase
    {
        private readonly IMailingService _mailingService;

        public MailingController(IMailingService mailingService)
        {
            _mailingService = mailingService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequestDto dto)
        {
            await _mailingService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body, dto.Attachments);
            return Ok(dto); 
                }
    }
}
