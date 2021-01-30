using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.RequestHandlers
{
    public abstract class Handler<T> : IHandler
    {
        public abstract Response Handle(T request);

        public Response Handle(object request)
        {
            return Handle((T)request);
        }
    }

    //Used to bypass type safety (nessecary as get request type from network)
    public interface IHandler
    {
        Response Handle(object request);
    }
}
