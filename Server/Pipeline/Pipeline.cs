using Server.RequestHandlers;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Pipelining
{
    public class Pipeline
    {
        private List<IMiddleware> middlewares = new List<IMiddleware>();

        public void AddMiddleware(IMiddleware middleware)
        {
            middlewares.Add(middleware);
        }

        public Response SendRequest(object request, IHandler handler)
        {
            //Uncomment for a more stable play experience
            //try
            //{
                foreach (IMiddleware middleware in middlewares)
                    middleware.Handle(request);

                return handler.Handle(request);
            //}catch(Exception e)
            //{
            //    Console.WriteLine($"Exception: " + e.Message);
            //    return Response.Fail("Exception happened on the server");
            //}
        }
    }
}
