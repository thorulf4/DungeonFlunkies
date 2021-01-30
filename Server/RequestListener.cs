using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Server.Pipeline;
using Server.RequestHandlers;
using Shared;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class RequestListener
    {
        private Dictionary<Type, Type> registeredRequests = new Dictionary<Type, Type>();
        private TcpListener tcpListener;
        private IServiceProvider serviceProvider;
        private Mediator mediator;
        private Type interactionHandler;

        public RequestListener(ushort port)
        {
            mediator = new Mediator();
            tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public ListenerBuilder Configure()
        {
            return new ListenerBuilder(this);
        }

        private void RegisterRequest<TR, TH>()
        {
            registeredRequests.Add(typeof(TR), typeof(TH));
        }

        private void StartListening()
        {

            tcpListener.Start();

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();

                ThreadPool.QueueUserWorkItem(HandleConnection, client);
            }
        }

        private void HandleConnection(object obj)
        {
            TcpClient client = obj as TcpClient;

            Stopwatch watch = Stopwatch.StartNew();
            long timeout = 1000 * 60 * 10; // 10 minutes

            using (NetworkStream stream = client.GetStream())
            {
                while (watch.ElapsedMilliseconds <= timeout)
                {
                    if (stream.DataAvailable)
                    {
                        HandleRequest(stream);

                        watch.Restart();
                    }
                }

                stream.Flush();
            }

            client.Close();
        }

        private void HandleRequest(NetworkStream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, leaveOpen: true))
            {
                string[] request = reader.ReadString().Split("\n");
                string type = request[0];
                string json = request[1];

                Response response = CallRequestHandler(type, json);

                if (response.HasData)
                    SendResponse(response, stream);
            }
        }

        private void SendResponse(Response response, Stream stream)
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true))
            {
                writer.Write(response.DataString);
            }
        }

        private Response CallRequestHandler(string strType, string json)
        {
            Type type = Type.GetType(strType);
            object data = JsonConvert.DeserializeObject(json, type);

            if(data is InteractionRequest interaction)
            {
                var handler = (IHandler)serviceProvider.GetRequiredService(interactionHandler);

                return mediator.SendRequest(data, handler);
            }
            else
            {
                var handlerType = registeredRequests[type];
                var handler = (IHandler)serviceProvider.GetRequiredService(handlerType);

                return mediator.SendRequest(data, handler);
            }

            
        }

        public class ListenerBuilder
        {
            private RequestListener subject;
            private bool isBuilt;
            private IServiceCollection services;

            public ListenerBuilder(RequestListener subject)
            {
                this.subject = subject;
                isBuilt = false;

                services = new ServiceCollection();
            }

            public ListenerBuilder BuildDependencies(Action<IServiceCollection> addDependencies)
            {
                IsBuiltCheck();
                addDependencies(services);
                return this;
            }

            private void IsBuiltCheck()
            {
                if (isBuilt)
                    throw new Exception("Listener has already started and therefor cannot be configured further");
            }

            public ListenerBuilder AddHandler<TR, TH>() where TH : Handler<TR>
            {
                IsBuiltCheck();

                services.AddTransient<TH>();
                subject.RegisterRequest<TR, TH>();

                return this;
            }

            public void StartListening()
            {
                IsBuiltCheck();
                isBuilt = true;

                var serviceProvider = services.BuildServiceProvider();
                subject.AddServiceProvider(serviceProvider);

                subject.StartListening();
            }

            public ListenerBuilder AddMiddleware(IMiddleware middleware)
            {
                IsBuiltCheck();

                subject.mediator.AddMiddleware(middleware);
                return this;
            }

            public ListenerBuilder AddInteractionHandler<T>() where T : class
            {
                IsBuiltCheck();

                services.AddTransient<T>();
                subject.interactionHandler = typeof(T);

                return this;
            }
        }

        private void AddServiceProvider(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}
