using Shared.Alerts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public interface IAlerter
    {
        void RegisterUser(string username);
        void SendAlerts<T>(T alert, ICollection<string> receivers) where T : Alert;
    }
}
