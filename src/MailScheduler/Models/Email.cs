using System;
using TemplateMailSender.Core.MailSender;

namespace MailScheduler.Models
{
    public sealed class Email
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string UserId { get; init; }
        public DateTime SendDate { get; init; }
        public EmailMessage Message { get; init; }
        public SendState SendState { get; set; } = SendStates.NotSend;
    }
}