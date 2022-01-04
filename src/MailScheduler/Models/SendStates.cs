namespace MailScheduler.Models
{
    public class SendStates
    {
        private static readonly SendState[] States = new[]
        {
            new SendState{ Id = 0, Name = "Send", Description = "Email successfully sended to all address's"},
            new SendState{ Id = 0, Name = "OnSend", Description = "Email on send"},
            new SendState{ Id = 0, Name = "NotSend", Description = "Email on send"},
        };

        public static SendState Send => States[0];
        public static SendState OnSend => States[1];
        public static SendState NotSend => States[2];
    }
}