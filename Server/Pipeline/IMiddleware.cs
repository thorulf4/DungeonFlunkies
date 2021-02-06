using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Pipelining
{
    public interface IMiddleware
    {
        void Handle(object request);
    }
}
