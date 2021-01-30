using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Pipeline
{
    public interface IMiddleware
    {
        void Handle(object request);
    }
}
