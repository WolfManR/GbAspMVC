using System;
using System.Collections.Generic;
using System.Linq;
using MailScheduler.Models;

namespace MailScheduler.Services
{
    public class EmailsRepository
    {
        private static readonly List<Email> _emails = new List<Email>();

        public IReadOnlyCollection<Email> EmailsToSend()
        {
            var currentDate = DateTime.Now;
            return _emails.Where(email => email.SendState == SendStates.NotSend && email.SendDate > currentDate).ToList();
        }

        public void AddEmailToSend(Email email)
        {
            email.Id = Guid.NewGuid();
            _emails.Add(email);
        }
    }
}