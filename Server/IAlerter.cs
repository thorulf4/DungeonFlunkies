using Shared.Alerts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public interface IAlerter
    {
        //TODO: Should probably use player ids over usernames to determine receivers
        void RegisterUser(string username);
        void SendAlerts<T>(T alert, ICollection<string> receivers) where T : Alert;
        void SendAlert<T>(T alert, string receiver) where T : Alert;
    }
}
