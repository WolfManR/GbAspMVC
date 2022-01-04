using MailScheduler.Models;
using MailScheduler.Services;

using Microsoft.AspNetCore.Mvc;

using System.Linq;
using TemplateMailSender.Core;

namespace MailScheduler.Controllers
{
    [Route("mails")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        private readonly EmailsRepository _emailsRepository;

        public MailsController(EmailsRepository emailsRepository)
        {
            _emailsRepository = emailsRepository;
        }

        [HttpGet("foruser/{userid}")]
        public IActionResult GetUserEmails(string userId)
        {
            var userMails = _emailsRepository.UserMails(userId);
            var data = userMails.Select(email => new MailInfo()
            {
                Message = email.Message,
                SendDate = email.SendDate,
                SendState = email.SendState.Name,
                SendStateDescription = email.SendState.Description
            }).ToList();

            return Ok(data);
        }
    }
}
