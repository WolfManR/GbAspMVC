namespace TemplateMailSender.Core.MailSender
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage, EmailConfiguration emailConfiguration);
    }
}