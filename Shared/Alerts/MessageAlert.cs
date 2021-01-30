using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Alerts
{
    public class MessageAlert : Alert
    {
        public string Sender { get; set; }
        public string Message { get; set; }

        public MessageAlert(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }
    }
}
