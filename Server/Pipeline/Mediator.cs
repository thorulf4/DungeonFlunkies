using Server.RequestHandlers;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Pipeline
{
    public class Mediator
    {
        private List<IMiddleware> middlewares = new List<IMiddleware>();

        public void AddMiddleware(IMiddleware middleware)
        {
            middlewares.Add(middleware);
        }

        public Response SendRequest(object request, IHandler handler)
        {
            foreach (IMiddleware middleware in middlewares)
                middleware.Handle(request);

            return handler.Handle(request);
        }
    }
}
