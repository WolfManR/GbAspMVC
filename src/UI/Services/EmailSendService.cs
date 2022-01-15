using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TemplateMailSender.Core.MailSender;

namespace UI.Services
{
    internal sealed class EmailSendService : BaseHttpClientService
    {
        public EmailSendService(HttpClient client, ILogger<BaseHttpClientService> logger) : base(client, logger)
        {
        }

        public async Task SendImmediately(EmailMessage email)
        {
            var json = JsonSerializer.Serialize(email);
            var request = new HttpRequestMessage(HttpMethod.Post, "emailsend/immediately")
            {
                Content = new StringContent(json)
            };
            var response = await Send(request);
            if (!response.IsSuccessStatusCode)
            {
                // TODO Fail notify
            }
        }

        public async Task ScheduleSend(EmailMessage email, DateTime date)
        {
            var json = JsonSerializer.Serialize(email);
            var request = new HttpRequestMessage(HttpMethod.Post, $"emailsend/scheduled/on/{date}")
            {
                Content = new StringContent(json)
            };
            var response = await Send(request);
            if (!response.IsSuccessStatusCode)
            {
                // TODO Fail notify
            }
        }
    }
}