using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Pipeline
{
    public class Debugger : IMiddleware
    {
        public void Handle(object request)
        {
            if(request is AuthenticatedRequest auth)
            {
                Console.WriteLine($"{auth.Name} during session {auth.SessionId.ToString().Replace("\n", "?")} has sent a {request.GetType().Name}");
            }
            else
            {
                Console.WriteLine($"Unknown user sent a {request.GetType().Name}");
            }
        }
    }
}
