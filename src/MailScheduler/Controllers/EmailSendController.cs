using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TemplateMailSender.Core.MailSender;

namespace MailScheduler.Controllers
{
	[Route("emailsend")]
	[ApiController]
	public class EmailSendController : ControllerBase
	{
        private readonly IEmailService _emailService;
        private readonly EmailConfiguration _domainConfiguration;

        public EmailSendController(IEmailService emailService, IOptions<EmailConfiguration> domainConfiguration)
        {
            _emailService = emailService;
            _domainConfiguration = domainConfiguration.Value;
        }

        [HttpPost("immediately")]
        public IActionResult SendImmediately([FromBody] EmailMessage message)
        {
            _emailService.Send(message, _domainConfiguration);
            return Ok();
        }

        [HttpPost("scheduled/on/{sendDate:datetime}")]
        public IActionResult Schedule([FromBody] EmailMessage message, [FromRoute] DateTime sendDate)
        {
            return Ok();
        }
	}
}
