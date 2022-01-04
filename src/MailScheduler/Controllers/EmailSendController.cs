using System;
using MailScheduler.Models;
using MailScheduler.Services;
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
        private readonly EmailsRepository _emailsRepository;
        private readonly EmailConfiguration _domainConfiguration;

        public EmailSendController(IEmailService emailService, IOptions<EmailConfiguration> domainConfiguration, EmailsRepository emailsRepository)
        {
            _emailService = emailService;
            _emailsRepository = emailsRepository;
            _domainConfiguration = domainConfiguration.Value;
        }

        [HttpPost("immediately")]
        public IActionResult SendImmediately([FromBody] EmailMessage message)
        {
            _emailService.Send(message, _domainConfiguration);
            _emailsRepository.AddEmailToSend(new Email()
            {
                SendState = SendStates.Send,
                Message = message,
                SendDate = DateTime.Now,
                UserId = string.Empty
            });
            return Ok();
        }

        [HttpPost("scheduled/on/{sendDate:datetime}")]
        public IActionResult Schedule([FromBody] EmailMessage message, [FromRoute] DateTime sendDate)
        {
            if (sendDate < DateTime.Now) return SendImmediately(message);

            _emailsRepository.AddEmailToSend(new Email()
            {
                Message = message,
                SendDate = sendDate,
                UserId = string.Empty
            });
            return Ok();
        }
	}
}
