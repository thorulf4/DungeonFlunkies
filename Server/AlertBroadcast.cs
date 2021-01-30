using Shared.Alerts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class AlertBroadcast
    {
        public List<string> Receivers { get; set; }
        public object Content { get; set; }

        public AlertBroadcast(List<string> receivers, Alert content)
        {
            Receivers = receivers;
            Content = content;
        }
    }
}
