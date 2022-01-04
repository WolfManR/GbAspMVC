using System;
using TemplateMailSender.Core.MailSender;

namespace TemplateMailSender.Core
{
    public class MailInfo
    {
        public DateTime SendDate { get; init; }
        public EmailMessage Message { get; init; }
        public string SendState { get; init; }
        public string SendStateDescription { get; init; }
    }
}